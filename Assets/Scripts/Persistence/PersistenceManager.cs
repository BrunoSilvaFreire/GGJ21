using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lunari.Tsuki.Runtime;
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

    [Serializable]
    public class PersistenceDataLookup : SerializableDictionary<int, PersistenceData> { }

    public class PersistenceManager : MonoBehaviour {
        public InstanceIdLookup objectLookup;
        public PersistenceDataLookup dataLookup;
        public UnityEvent onLoad, onSave;
        public Object FindObjectFromInstanceID(int iid) {
            return null;
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
        [ShowInInspector]
        public void TestRun() {
            Initialize();
            Save();
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
    }
}