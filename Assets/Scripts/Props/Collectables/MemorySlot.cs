using GGJ.Master;
using GGJ.Master.UI;
using GGJ.Traits;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Entities;
using Props.Collectables;
using UI;
using UnityEngine;

namespace GGJ.Props.Collectables {
    public class MemorySlot : Collectable, IPersistant{
        private bool m_savedActive;
        private Transform m_savedParent;
        private Vector3 m_savedPosition;
        private Collector m_savedCollector, m_collector;
        private PersistanceManager m_manager;
        public float showUIForSeconds = 3;
        protected override CollectionAction ProcessCollection(Entity entity) {
            if (entity.Access<Collector>(out var collector)) {
                collector.Collect(this);
                transform.parent = collector.Owner.transform.parent;
                m_collector = collector;
                PlayerUI.Instance.KnowledgeEditor.indicator.SetShownForDuration(true, showUIForSeconds);
                return CollectionAction.Ok;
            }
            return CollectionAction.None;
        }

        private void FixedUpdate() {
            if (m_collector) {
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
                Player.Instance.Pawn.GetTrait<Knowledgeable>().MaxNumberOfKnowledge.Value--;
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