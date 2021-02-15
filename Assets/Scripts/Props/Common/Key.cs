using GGJ.Persistence;
using GGJ.Props.Collectables;
using GGJ.Props.Traits;
using Lunari.Tsuki.Entities;
using UnityEngine;
namespace GGJ.Props.Common {
    public class Key : Collectable, IPersistant {

        private PersistentProperty<bool> m_savedActive;
        private PersistentProperty<Vector3> m_savedPosition;
        private PersistentProperty<Transform> m_savedParent;
        private PersistentProperty<Collector> m_savedCollector;
        private Collector m_collector;
        public void Configure(PersistenceData data) {
            data.Bind(nameof(m_savedActive), out m_savedActive, value => gameObject.SetActive(value));
            data.Bind(nameof(m_savedCollector), out m_savedCollector, value => {
                m_collector = value;
                if (m_collector != null) {
                    m_collector.Collect(this);
                }
            });
            data.Bind(nameof(m_savedPosition), out m_savedPosition, value => {
                transform.position = value;
            });
            data.Bind(nameof(m_savedParent), out m_savedParent, value => {
                transform.SetParent(value);
            });
        }
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
    }
}