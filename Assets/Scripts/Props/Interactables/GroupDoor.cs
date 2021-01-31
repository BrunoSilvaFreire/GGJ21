using DG.Tweening;
using GGJ.Master;
using GGJ.Props;
using Lunari.Tsuki.Entities;
using UnityEngine;
using World;

namespace Props.Interactables {
    public class GroupDoor : Trait, ITiledObject, IButtonGroup, IPersistant {

        [SerializeField] private int m_buttonGroupId;

        private bool m_isOpen, m_savedIsOpen;
        private Vector3 m_savedScale;
        
        private ButtonGroupManager m_buttonGroupManager;
        private PersistanceManager m_persistenceManager;
        
        public void Setup(ObjectData data) {
            m_buttonGroupId = PropertyData.GetInt(data.properties, "id");
        }

        public void ConfigureGroup(ButtonGroupManager manager) {
            m_buttonGroupManager = manager;
            m_buttonGroupManager.AddListenerToButtonGroup(m_buttonGroupId, OnGroupCleared);
        }

        public void OnGroupCleared() {
            transform.DOScale(Vector3.zero, 1);
            m_isOpen = true;
        }
        
        public void ConfigurePersistance(PersistanceManager manager) {
            m_persistenceManager = manager;
            m_persistenceManager.onLoad.AddListener(OnLoad);
            m_persistenceManager.onSave.AddListener(OnSave);
            OnSave();
        }
        
        public void OnDestroy() {
            if (m_persistenceManager) {
                m_persistenceManager.onLoad.RemoveListener(OnLoad);
                m_persistenceManager.onSave.RemoveListener(OnSave);
            }
        }
        
        private void OnSave() {
            m_savedScale = transform.localScale;
            m_savedIsOpen = m_isOpen;
        }

        private void OnLoad() {
            m_isOpen = m_savedIsOpen;
            transform.localScale = m_savedScale;
        }
    }
}