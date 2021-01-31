using System;
using System.Collections.Generic;
using Lunari.Tsuki.Runtime.Misc;
using Lunari.Tsuki.Runtime.Singletons;
using Props.Interactables;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;
using World;

namespace GGJ.Props {
    
    
    [Serializable]
    public class SerializableIButtonGroup : SerializableInterface<IButtonGroup> {}
    
    public class ButtonGroupManager : Singleton<ButtonGroupManager>, ITiledMap {

        private class ButtonGroup {
            public int quantity;
            public UnityEvent groupClearedEvent = new UnityEvent();
        }
        
        private readonly Dictionary<int, ButtonGroup> m_buttonGroup2Quantity = new Dictionary<int, ButtonGroup>();

        [SerializeField, HideInInspector] private List<SerializableIButtonGroup> m_groupObjects;
        
        //used to bake group objects
        public void Setup(MapData data) {
            var objects = FindObjectsOfType<UnityEngine.Object>(true);
            foreach (var obj in objects) {
                if (obj is IButtonGroup group) {
                    m_groupObjects.Add(new SerializableIButtonGroup{Value = group});
                }
            }
        }
        
        public void AddToButtonGroup(int buttonGroup) {
            if (!m_buttonGroup2Quantity.ContainsKey(buttonGroup)) {
                m_buttonGroup2Quantity[buttonGroup] = new ButtonGroup();
            }
            m_buttonGroup2Quantity[buttonGroup].quantity++;
        }

        public void AddListenerToButtonGroup(int buttonGroup, UnityAction onGroupCleared) {
            if (!m_buttonGroup2Quantity.ContainsKey(buttonGroup)) {
                m_buttonGroup2Quantity[buttonGroup] = new ButtonGroup();
            }
            m_buttonGroup2Quantity[buttonGroup].groupClearedEvent.AddListener(onGroupCleared);
        }

        public void RemoveFromButtonGroup(int buttonGroup) {
            var group = m_buttonGroup2Quantity[buttonGroup];
            group.quantity--;
            if (group.quantity <= 0) {
                group.groupClearedEvent.Invoke();
            }
        }

        private void Start() {
            m_groupObjects.ForEach(obj => {
                obj.Value.ConfigureGroup();
            });
        }
    }
}