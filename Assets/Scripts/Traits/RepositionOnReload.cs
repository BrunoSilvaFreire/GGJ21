using System;
using GGJ.Master;
using Lunari.Tsuki.Entities;
using UnityEngine;
namespace GGJ.Traits {
    public class RepositionOnReload : Trait, IPersistant {

        public bool resetToInitialPosition;
        
        private PersistanceManager m_manager;
        private Vector3 position;
 
        public void ConfigurePersistance(PersistanceManager manager) {
            m_manager = manager;
            m_manager.onLoad.AddListener(OnLoad);
            m_manager.onSave.AddListener(OnSave);
        }

        private void OnSave() {
            if (!resetToInitialPosition) {
                position = Owner.transform.position;
            }
        }

        private void OnLoad() {
            Owner.transform.position = position;
        }

        private void OnDestroy() {
            if (m_manager) {
                m_manager.onLoad.RemoveListener(OnLoad);
                m_manager.onSave.RemoveListener(OnSave);
            }
        }
    }
}