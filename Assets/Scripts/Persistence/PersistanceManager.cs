using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        public float timeUntilClose = .5F;
        public float timeUntilLoad = .5F;
        public float minWaitTime = .5F;

        public UnityEvent onLoad;
        public UnityEvent onSave;

        [SerializeField] private List<UnityEngine.Object> m_persistantObjects;
        
        private Coroutine restartRoutine;

        public void Restart() {
            Coroutines.ReplaceCoroutine(ref restartRoutine, this, RestartRoutine());
        }
        private IEnumerator RestartRoutine() {
            var ui = PlayerUI.Instance;
            yield return new WaitForSecondsRealtime(timeUntilClose);
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
                var cam = Player.Instance.camera;
                var pos = p.transform.position;
                if (p.Access(out Filmed filmed)) {
                    var transposer = filmed.Camera.GetCinemachineComponent<CinemachineFramingTransposer>();
                    if (transposer != null) {
                        pos.z = -transposer.m_CameraDistance;
                    }
                }
                cam.transform.position = pos;
            }
            ui.deathCurtains.Hide();
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
            var objects = GetComponentsInChildren<IPersistant>(true);
            foreach (var obj in objects) {
                m_persistantObjects.Add(obj as UnityEngine.Object);
            }
        }

        private void Start() {
            m_persistantObjects.ForEach(obj => {
                (obj as IPersistant).ConfigurePersistance(this);
            });
        }
    }
}