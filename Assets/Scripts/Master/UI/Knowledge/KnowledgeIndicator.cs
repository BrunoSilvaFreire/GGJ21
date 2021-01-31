using GGJ.Traits;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Exceptions;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace GGJ.Master.UI.Knowledge {
    public class KnowledgeIndicator : UIBehaviour {
        private TraitBind<Knowledgeable> knowledgeable;
        [AssetsOnly]
        public KnowledgeView prefab;
        private KnowledgeView[] views;

        public KnowledgeView[] Views => views;
        public UnityEvent onViewsAssigned;
        protected override void Awake() {
            knowledgeable = Player.Instance.Bind<Knowledgeable>();
            knowledgeable.OnBound(Reload);
        }
        private void Reload(Knowledgeable knowledgeable) {
            if (knowledgeable == null) {
                throw new WTFException("Null knowledgeable");
            }
            if (views != null) {
                if (views.Length != knowledgeable.MaxNumberOfKnowledge) {
                    foreach (var knowledgeView in views) {
                        Destroy(knowledgeView.gameObject);
                    }
                    Reallocate(knowledgeable);
                }
            } else {
                Reallocate(knowledgeable);
            }
            knowledgeable.onMaxKnowledgeChanged.AddDisposableListener(() => Reload(knowledgeable)).FireOnce().DisposeOn(this.knowledgeable.onBound);
            knowledgeable.onKnowledgeChanged.AddDisposableListener(() => Reload(knowledgeable)).FireOnce().DisposeOn(this.knowledgeable.onBound);
            var current = 0;
            for (var i = 0; i < knowledgeable.MaxNumberOfKnowledge; i++) {
                Knowledgeable.Knowledge knowledge;
                do {
                    knowledge = (Knowledgeable.Knowledge)(1 << current++);
                } while (!knowledgeable.Matches(knowledge) && current < 16);
                views[i].Setup(knowledge);
            }
        }
        private void Reallocate(Knowledgeable knowledgeable) {
            views = new KnowledgeView[knowledgeable.MaxNumberOfKnowledge];
            for (var i = 0; i < knowledgeable.MaxNumberOfKnowledge; i++) {
                views[i] = prefab.Clone(transform);
            }
            onViewsAssigned.Invoke();
        }
    }
}