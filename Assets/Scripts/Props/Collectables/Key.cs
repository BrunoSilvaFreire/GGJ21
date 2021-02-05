using System;
using GGJ.Master;
using GGJ.Traits;
using Lunari.Tsuki.Entities;
using UnityEngine;

namespace Props.Collectables {
    public class Key : Collectable, IPersistant {

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
                return CollectionAction.Ok;
            }
            return CollectionAction.None;
        }

        private void FixedUpdate() {
            if (m_collector) {
                transform.position = Vector3.Lerp(transform.position, m_collector.Owner.transform.position, 0.1f);
            }
        }

        public void Consume() {
            gameObject.SetActive(false);
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
            OnSave(); //saves current state
        }

        private void OnLoad() {
            transform.position = m_savedPosition;
            transform.parent = m_savedParent;
            m_collector = m_savedCollector;
            gameObject.SetActive(m_savedActive);
            collected = m_collector != null;
            if (m_collector != null) {
                m_collector.Collect(this);
            }
        }

        private void OnSave() {
            m_savedParent = transform.parent;
            m_savedPosition = transform.position;
            m_savedCollector = m_collector;
            m_savedActive = gameObject.activeSelf;
        }
    }
}