using System;
using DG.Tweening;
using GGJ.Master;
using GGJ.Traits;
using Lunari.Tsuki.Entities;
using Props.Collectables;
using UnityEngine;

namespace Props.Interactables {
    public class ElectricDoor : Trait, IPersistant {
        
        private bool m_savedActive;
        private Vector3 m_savedScale;
        
        private PersistanceManager m_manager;
        
        private AnimatorBinder m_binder;
        
        public override void Configure(TraitDependencies dependencies) {
            dependencies.DependsOn(out m_binder);
        }

        public void Open(Key key) {
            m_binder.Animator.SetTrigger("open");
            transform.DOScale(Vector3.zero, 1f).OnComplete(() => gameObject.SetActive(false));
            key.Consume();
        }

        public void ConfigurePersistance(PersistanceManager manager) {
            m_manager = manager;
            m_manager.onLoad.AddListener(OnLoad);
            m_manager.onSave.AddListener(OnSave);
        }

        public void OnDestroy() {
            if (m_manager) {
                m_manager.onLoad.RemoveListener(OnLoad);
                m_manager.onSave.RemoveListener(OnSave);    
            }
        }

        private void OnSave() {
            m_savedActive = gameObject.activeSelf;
            m_savedScale = transform.localScale;
        }

        private void OnLoad() {
            gameObject.SetActive(m_savedActive);
            transform.localScale = m_savedScale;
        }
    }
}