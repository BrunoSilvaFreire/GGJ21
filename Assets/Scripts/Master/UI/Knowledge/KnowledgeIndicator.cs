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

        public KnowledgeView[] Views {
            get;
            private set;
        }

        public UnityEvent onViewsAssigned;
        protected override void Awake() {
            knowledgeable = Player.Instance.Bind<Knowledgeable>();
            knowledgeable.OnBound(Reload);
            knowledgeable.BindToValue(
                knowledgeable1 => knowledgeable1.CurrentKnowledge,
                delegate {
                    Reload(knowledgeable.Current);
                }
            );
        }
        private void Reload(Knowledgeable knowledgeable) {
            if (knowledgeable == null) {
                throw new WTFException("Null knowledgeable");
            }
            if (Views != null) {
                if (Views.Length != knowledgeable.MaxNumberOfKnowledge) {
                    foreach (var knowledgeView in Views) {
                        Destroy(knowledgeView.gameObject);
                    }
                    Reallocate(knowledgeable);
                }
            } else {
                Reallocate(knowledgeable);
            }
            ;
            knowledgeable.MaxNumberOfKnowledge.onChanged.AddDisposableListener(() => Reload(knowledgeable)).FireOnce().DisposeOn(this.knowledgeable.onBound);
            var current = 0;
            for (var i = 0; i < knowledgeable.MaxNumberOfKnowledge; i++) {
                Knowledgeable.Knowledge knowledge;
                do {
                    knowledge = (Knowledgeable.Knowledge)(1 << current++);
                } while (!knowledgeable.Matches(knowledge) && current < 16);
                var toUse = knowledgeable.Matches(knowledge) ? knowledge : Knowledgeable.Knowledge.None;
                Views[i].Setup(toUse);
            }
        }
        private void Reallocate(Knowledgeable knowledgeable) {
            Views = new KnowledgeView[knowledgeable.MaxNumberOfKnowledge];
            for (var i = 0; i < knowledgeable.MaxNumberOfKnowledge; i++) {
                var view = prefab.Clone(transform);
                Views[i] = view;
                view.Setup(Knowledgeable.Knowledge.None);
            }
            onViewsAssigned.Invoke();
        }
    }
}