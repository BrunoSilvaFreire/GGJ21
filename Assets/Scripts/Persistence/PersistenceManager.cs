using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace GGJ.Persistence {

    [Serializable]
    public class SaveTree : SerializableDictionary<int, SaveData> { }

    [Serializable]
    public class SaveData {
        [SerializeReference]
        public List<PersistentProperty> properties = new List<PersistentProperty>();
    }

    [Serializable]
    public class InstanceIdLookup : SerializableDictionary<int, Object> { }

    public class PersistenceDataLookup : SerializableDictionary<int, PersistenceData> { }
    public delegate IEnumerator RestartListener();
    public class PersistenceManager : Singleton<PersistenceManager> {
        public InstanceIdLookup objectLookup;
        [NonSerialized]
        public PersistenceDataLookup dataLookup;
        public UnityEvent onLoad, onSave;
        private List<RestartListener> beforeRestart, afterRestart;
        private void Awake() {
            beforeRestart = new List<RestartListener>();
            afterRestart = new List<RestartListener>();
        }
        private Coroutine restartRoutine;
        public Coroutine Restart() {
            if (restartRoutine == null) {
                Coroutines.ReplaceCoroutine(ref restartRoutine, this, RestartRoutine());
            }
            return restartRoutine;
        }
        private IEnumerator RestartRoutine() {
            foreach (var restartListener in beforeRestart) {
                yield return restartListener();
            }
            Load();
            foreach (var restartListener in afterRestart) {
                yield return restartListener();
            }
            restartRoutine = null;
        }
        private void Start() {
            InitializeLegacy();
        }
        private void Load() {
            onLoad.Invoke();
        }

        public void Save() {
            onSave.Invoke();

            var root = new SaveTree();
            foreach (var persistant in activePersistant) {
                var childData = new SaveData();
                var data = GetDataOf(persistant);
                foreach (var persistentProperty in data.Properties) {
                    var prop = persistentProperty.AsSafeProperty();
                    if (prop != null) {
                        childData.properties.Add(prop);
                    }
                }
                root[persistant.GetInstanceID()] = childData;
            }
            var buf = JsonUtility.ToJson(root);
            File.WriteAllText(SaveDataPath, buf);
            Debug.Log($"Saved to {SaveDataPath}");
        }

        private List<IPersistant> activePersistant = new List<IPersistant>();
        private void InitializeLegacy() {
            foreach (var persistantLegacy in FindObjectsOfType<MonoBehaviour>().OfType<IPersistantLegacy>()) {
                Debug.LogWarning($"Found object {persistantLegacy} using Obsolete IPersistantLegacy. Remember to yeet this cunt before building.", (Object)persistantLegacy);
                persistantLegacy.ConfigurePersistance(this);
            }

        }
        private void Initialize() {
            var persistantObjects = FindObjectsOfType<MonoBehaviour>().OfType<IPersistant>().ToList();
            foreach (var persistant in persistantObjects) {
                if (persistant is Object obj) {
                    objectLookup[persistant.GetInstanceID()] = obj;
                }
            }
            foreach (var persistant in persistantObjects) {
                var data = GetDataOf(persistant);
                persistant.Configure(data);
            }
            activePersistant.AddRange(persistantObjects);
        }
        private PersistenceData GetDataOf(IPersistant asObject) {
            var id = asObject.GetInstanceID();
            if (!dataLookup.TryGetValue(id, out var value)) {
                value = new PersistenceData(id, this);
                dataLookup[id] = value;
            }
            return value;
        }
        public static string SaveDataPath => $"{Application.persistentDataPath}/save.json";
        public void BeforeRestart(RestartListener listener) {
            beforeRestart ??= new List<RestartListener>();
            beforeRestart.Add(listener);
        }
        public void AfterRestart(RestartListener listener) {
            afterRestart ??= new List<RestartListener>();
            afterRestart.Add(listener);
        }
    }
}