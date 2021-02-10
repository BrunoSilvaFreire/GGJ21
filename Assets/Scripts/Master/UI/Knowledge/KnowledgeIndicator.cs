using System;
using System.Collections.Generic;
using GGJ.Traits;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Exceptions;
using Lunari.Tsuki.Runtime.Stacking;
using Sirenix.OdinInspector;
using UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace GGJ.Master.UI.Knowledge {
    public class KnowledgeIndicator : AbstractCompositeView<KnowledgeView> {
        private TraitBind<Knowledgeable> knowledgeable;
        [AssetsOnly]
        public KnowledgeView prefab;


        public UnityEvent onViewsAssigned;
        protected void Awake() {
            shown.mode = BooleanStackable.Mode.All;
            knowledgeable = Player.Instance.Bind<Knowledgeable>();
            knowledgeable.Bind(OnKnowledgeableChanged);
        }
        private void OnKnowledgeableChanged(Knowledgeable value) {
            if (value == null) {
                throw new ArgumentException("Null knowledgeable", nameof(value));
            }
            if (subviews != null) {
                if (subviews.Count != value.MaxNumberOfKnowledge) {
                    foreach (var knowledgeView in subviews) {
                        Destroy(knowledgeView.gameObject);
                    }
                    Reallocate(value);
                }
            } else {
                Reallocate(value);
            }
            value.MaxNumberOfKnowledge.onChanged.AddDisposableListener(() => OnKnowledgeableChanged(value)).DisposeOn(knowledgeable.onBound);
            value.CurrentKnowledge.Bind(_ => UpdateKnowledgeView()).DisposeOn(knowledgeable.onBound);
            UpdateKnowledgeView();
        }
        private void UpdateKnowledgeView() {
            var to = knowledgeable.Current;
            var current = 0;
            for (var i = 0; i < to.MaxNumberOfKnowledge; i++) {
                Traits.Knowledge.Knowledge knowledge;
                do {
                    knowledge = (Traits.Knowledge.Knowledge)(1 << current++);
                } while (!to.Matches(knowledge) && current < sizeof(Traits.Knowledge.Knowledge) * 8);
                var toUse = to.Matches(knowledge) ? knowledge : Traits.Knowledge.Knowledge.None;
                subviews[i].Setup(toUse);
            }
        }
        private void Reallocate(Knowledgeable knowledgeable) {
            subviews ??= new List<KnowledgeView>();
            subviews.Clear();

            for (var i = 0; i < knowledgeable.MaxNumberOfKnowledge; i++) {
                subviews.Add(prefab.Clone(transform));
            }
            onViewsAssigned.Invoke();
        }
    }
}