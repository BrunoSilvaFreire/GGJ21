using System;
using System.Collections;
using System.Collections.Generic;
using GGJ.Master;
using GGJ.Traits;
using Lunari.Tsuki.Entities;
using Props.Collectables;
using UnityEngine;

namespace GGJ.Collectables {
    public class AthenaPart : Collectable, IPersistant {

        public int id;
        public bool follow = true;
        
        private bool m_savedActive;
        private Transform m_savedParent;
        private Vector3 m_savedPosition;
        private Collector m_savedCollector, m_collector;
        private PersistanceManager m_manager;
        
        protected override CollectionAction ProcessCollection(Entity entity) {
            if (entity.Access<Collector>(out var collector)) {
                collector.Collect(this);
                transform.parent = collector.Owner.transform.parent;
                m_collector = collector;
            }
            return CollectionAction.None;
        }

        private void Awake() {
            GameManager.Instance.RegisterAthenaPart(id);
        }

        private void FixedUpdate() {
            if (m_collector && follow) {
                transform.position = Vector3.Lerp(transform.position, m_collector.Owner.transform.position, 0.1f);
            }
        }

        private void OnDestroy() {
            if (m_manager) {
                m_manager.onSave.RemoveListener(OnSave);
                m_manager.onLoad.RemoveListener(OnLoad);
            }
        }

        public void ConfigurePersistance(PersistanceManager manager) {
            m_manager = manager;
            m_manager.onSave.AddListener(OnSave);
            m_manager.onLoad.AddListener(OnLoad);
            OnSave();//saves current state
        }

        private void OnLoad() {
            if (m_collector != m_savedCollector) {
                GameManager.Instance.RegisterAthenaPart(id);
            }
            transform.position = m_savedPosition;
            transform.parent = m_savedParent;
            m_collector = m_savedCollector;
            gameObject.SetActive(m_savedActive);
        }
        
        private void OnSave() {
            m_savedParent = transform.parent;
            m_savedPosition = transform.position;
            m_savedCollector = m_collector;
            m_savedActive = gameObject.activeSelf;
        }
    }
}
