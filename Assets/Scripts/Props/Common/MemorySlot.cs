using GGJ.Game;
using GGJ.Game.Traits;
using GGJ.Persistence;
using GGJ.Props.Collectables;
using GGJ.Props.Traits;
using GGJ.UI;
using GGJ.UI.Common;
using Lunari.Tsuki.Entities;
using UnityEngine;
namespace GGJ.Props.Common {
    public class MemorySlot : Collectable, IPersistantLegacy{
        private bool m_savedActive;
        private Transform m_savedParent;
        private Vector3 m_savedPosition;
        private Collector m_savedCollector, m_collector;
        private PersistenceManager m_manager;
        public float showUIForSeconds = 3;
        protected override CollectionAction ProcessCollection(Entity entity) {
            if (entity.Access<Collector>(out var collector)) {
                collector.Collect(this);
                transform.parent = collector.Owner.transform.parent;
                m_collector = collector;
                PlayerUI.Instance.knowledgeEditor.indicator.SetShownForDuration(true, showUIForSeconds);
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

        public void ConfigurePersistance(PersistenceManager manager) {
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