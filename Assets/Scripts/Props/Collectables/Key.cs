using System;
using GGJ.Traits;
using Lunari.Tsuki.Entities;
using UnityEngine;

namespace Props.Collectables {
    public class Key : Collectable {

        private Collector m_collector;
        protected override CollectionAction ProcessCollection(Entity entity) {
            if (entity.Access(out m_collector)) {
                m_collector.Collect(this);
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