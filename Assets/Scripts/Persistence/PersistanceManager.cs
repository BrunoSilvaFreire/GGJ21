using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cinemachine;
using GGJ.Master.UI;
using GGJ.Traits.Combat;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Misc;
using Lunari.Tsuki.Runtime.Singletons;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using World;

namespace GGJ.Master {

    public class PersistanceManager : MonoBehaviour, ITiledWorld {

        public float timeUntilLoad = .5F;
        public float minWaitTime = .5F;

        public UnityEvent onLoad;
        public UnityEvent onSave;

        [SerializeField] private List<UnityEngine.Object> m_persistantObjects;

        private Coroutine restartRoutine;

        public Coroutine Restart() {
            if (restartRoutine == null) {
                Coroutines.ReplaceCoroutine(ref restartRoutine, this, RestartRoutine());
            }
            return restartRoutine;
        }
        private IEnumerator RestartRoutine() {
            var ui = PlayerUI.Instance;
            var time = Time.time;
            ui.deathCurtains.Show();
            yield return new WaitForSecondsRealtime(timeUntilLoad);
            Load();
            Time.timeScale = 1;
            var remaining = minWaitTime - (Time.time - time);
            if (remaining > 0) {
                yield return new WaitForSecondsRealtime(remaining);
            }
            var p = Player.Instance.Pawn;
            if (p != null) {
                if (p.Access(out Living living)) {
                    living.Alive = true;
                }
                if (p.Access(out Filmed filmed)) {
                    filmed.MoveToEntity();
                } else {
                    Player.Instance.camera.transform.position = p.transform.position;
                }
            }
            ui.deathCurtains.Hide();
            restartRoutine = null;
        }

        private void Load() {
            onLoad.Invoke();
        }

        public void Save() {
            onSave.Invoke();
        }

        [ShowInInspector]
        public void Setup() {
            m_persistantObjects.Clear();
            var objects = FindObjectsOfType<MonoBehaviour>(true).OfType<IPersistant>();
            foreach (var obj in objects) {
                m_persistantObjects.Add(obj as UnityEngine.Object);
            }
        }

        private void Start() {
            m_persistantObjects.ForEach(obj =>
            {
                if (obj is IPersistant p) {
                    p.ConfigurePersistance(this);
                }
            });
        }
    }
}