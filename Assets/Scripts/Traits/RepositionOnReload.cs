using System;
using Cinemachine;
using GGJ.Game;
using GGJ.Persistence;
using Lunari.Tsuki.Entities;
using UnityEngine;
namespace GGJ.Traits {
    public class RepositionOnReload : Trait, IPersistantLegacy {

        public bool resetToInitialPosition;

        private PersistenceManager m_manager;
        private Vector3 position;

        public void ConfigurePersistance(PersistenceManager manager) {
            m_manager = manager;
            m_manager.onLoad.AddListener(OnLoad);
            m_manager.onSave.AddListener(OnSave);
            position = Owner.transform.position;
        }
        public override void Configure(TraitDependencies dependencies) {
            position = dependencies.Entity.transform.position;
        }

        private void OnSave() {
            if (!resetToInitialPosition) {
                position = Owner.transform.position;
            }
        }

        private void OnLoad() {
            Owner.transform.position = position;
            if (Owner == Player.Instance.Pawn) {
                var player = Player.Instance;
                player.transform.position = position;
                var ev = player.cameraBrain.m_CameraActivatedEvent;

                void Callback(ICinemachineCamera arg0, ICinemachineCamera arg1) {
                    if (arg0 is CinemachineVirtualCamera cam) {
                        var transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
                        if (transposer != null) {
                            cam.gameObject.SetActive(false);
                            var pos = position;
                            pos.z = -transposer.m_CameraDistance;
                            cam.transform.position = pos;
                            cam.gameObject.SetActive(true);
                        }
                    }
                    ev.RemoveListener(Callback);
                }

                ev.AddListener(Callback);
            }
        }

        private void OnDestroy() {
            if (m_manager) {
                m_manager.onLoad.RemoveListener(OnLoad);
                m_manager.onSave.RemoveListener(OnSave);
            }
        }
    }
}