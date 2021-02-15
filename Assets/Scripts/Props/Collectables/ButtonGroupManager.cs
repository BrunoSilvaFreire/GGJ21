using System.Collections.Generic;
using GGJ.Props.Interactables;
using Lunari.Tsuki.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Props.Collectables {

    public class ButtonGroupManager : MonoBehaviour {

        public class ButtonGroup {
            public int quantity;
            public Color color = RandomColor();
            private static Color RandomColor() {
                var col = (ColorHSV)Colors.Random();
                col.S = 1;
                col.V = 1;
                return col;
            }
            public UnityEvent groupClearedEvent = new UnityEvent();
        }

        private readonly Dictionary<int, ButtonGroup> m_buttonGroup2Quantity = new Dictionary<int, ButtonGroup>();

        [SerializeField] private List<UnityEngine.Object> m_groupObjects;

        //used to bake group objects
        [ShowInInspector]
        public void Setup() {
            m_groupObjects.Clear();
            var objects = GetComponentsInChildren<IButtonGroup>();
            foreach (var obj in objects) {
                m_groupObjects.Add(obj as UnityEngine.Object);
            }
        }

        public ButtonGroup AddToButtonGroup(int buttonGroup) {
            ButtonGroup group;
            if (!m_buttonGroup2Quantity.ContainsKey(buttonGroup)) {
                group = new ButtonGroup();
                m_buttonGroup2Quantity[buttonGroup] = group;
            } else {
                group = m_buttonGroup2Quantity[buttonGroup];
            }
            group.quantity++;
            return group;
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
            m_groupObjects.ForEach(obj =>
            {
                (obj as IButtonGroup).ConfigureGroup(this);
            });
        }
        public ButtonGroup GetGroup(int mButtonGroupId) {
            return m_buttonGroup2Quantity[mButtonGroupId];
        }
    }
}