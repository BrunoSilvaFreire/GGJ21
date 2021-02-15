using System;
using System.Collections.Generic;
using GGJ.Game;
using GGJ.Game.Traits;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Stacking;
using Sirenix.OdinInspector;
using UnityEngine.Events;
namespace GGJ.UI.Common.Knowledge {
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
                Game.Knowledge knowledge;
                do {
                    knowledge = (Game.Knowledge)(1 << current++);
                } while (!to.Matches(knowledge) && current < sizeof(Game.Knowledge) * 8);
                var toUse = to.Matches(knowledge) ? knowledge : Game.Knowledge.None;
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