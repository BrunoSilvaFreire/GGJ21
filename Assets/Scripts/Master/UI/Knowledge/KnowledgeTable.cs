using System.Collections.Generic;
using Common;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Runtime;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.Extensions;

namespace GGJ.Master.UI.Knowledge {
    public class KnowledgeTable : MonoBehaviour {
        public KnowledgeView prefab;
        public View view;
        public TableLayoutGroup table;
        public UnityEvent onViewsAssigned;
        public CanvasGroup group;
        public List<KnowledgeView> Views {
            get;
            private set;
        }

        private void Start() {
            GameManager.Instance.onAvailableKnowledgeFound.AddListener(OnAvailableChanged);
            OnAvailableChanged();
        }
        private void OnAvailableChanged() {
            var available = GameManager.Instance.AvailableKnowledge;
            table.transform.ClearChildren();
            Views = new List<KnowledgeView>();
            for (var i = 0; i < sizeof(ushort) * 8; i++) {
                var candidate = (Knowledgeable.Knowledge)(1 << i);
                if ((available & candidate) == candidate) {
                    // Unlocked
                    var item=prefab.Clone(table.transform);
                    item.Setup(candidate);
                    Views.Add(item);
                }
            }
            onViewsAssigned.Invoke();
        }
    }
}