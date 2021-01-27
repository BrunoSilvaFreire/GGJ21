using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Exceptions;
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
        protected override void Awake() {
            knowledgeable = Player.Instance.Bind<Knowledgeable>((bind, arg0) =>
            {
                views = new KnowledgeView[arg0.maxNumberOfKnowledge];
                for (var i = 0; i < arg0.maxNumberOfKnowledge; i++) {
                    views[i] = prefab.Clone(transform);
                }
            });
            knowledgeable.OnBound(Reload);
        }
        private void Reload(Knowledgeable arg0) {
            if (arg0 == null) {
                throw new WTFException("Null knowledgeable");
            }
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