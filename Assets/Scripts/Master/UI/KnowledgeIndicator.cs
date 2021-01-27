using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Lunari.Tsuki.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using Traits;
using UnityEngine.EventSystems;

namespace GGJ.Master.UI {
    public class KnowledgeIndicator : UIBehaviour {
        private TraitBind<Knowledgeable> knowledgeable;
        [AssetsOnly]
        public KnowledgeView prefab;
        private KnowledgeView[] views;
        protected override void Start() {
            knowledgeable = Player.Instance.Bind<Knowledgeable>();
            knowledgeable.OnBound(delegate(Knowledgeable arg0)
            {
                views = new KnowledgeView[arg0.maxNumberOfKnowledge];
                for (var i = 0; i < arg0.maxNumberOfKnowledge; i++) {
                    views[i] = prefab.Clone(transform);
                }
            });
            knowledgeable.OnBound(Reload);
        }
        private void Reload(Knowledgeable arg0) {
            arg0.onKnowledgeChanged.AddDisposableListener(() => Reload(arg0)).FireOnce().DisposeOn(knowledgeable.onBound);
            var current = 0;
            for (var i = 0; i < arg0.maxNumberOfKnowledge; i++) {
                Knowledgeable.Knowledge knowledge;
                do {
                    knowledge = (Knowledgeable.Knowledge)(1 << current++);
                } while (!arg0.Matches(knowledge) && current < 16);
                views[i].Setup(knowledge);
            }
        }
    }
}