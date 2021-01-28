using System;
using GGJ.Traits;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Runtime;
using UnityEngine;
namespace GGJ.Master.UI.Knowledge {
    public class KnowledgeViewBinder : MonoBehaviour {
        public KnowledgeView view;
        private TraitBind<Knowledgeable> knowledgeable;
        private static readonly int Acquired = Animator.StringToHash("Acquired");
        private void OnValidate() {
            if (view == null) {
                return;
            }
            view.animator.EnsureHasParameter("Acquired", AnimatorControllerParameterType.Bool);
        }
        private void Start() {
            knowledgeable = Player.Instance.Bind<Knowledgeable>((arg0, knowledgeable1) => {
                arg0.OnBound(arg1 => view.animator.SetBool(Acquired, arg1.Matches(view.Knowledge)));
            });
        }
    }
}