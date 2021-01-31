using Common;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Runtime;
using Shiroi.FX.Effects;
using UI;
using UnityEngine;
using UnityEngine.UI;
namespace GGJ.Master.UI.Knowledge {
    public class KnowledgeView : AnimatedView, ISetupable<Knowledgeable.Knowledge> {
        public Image svg;
        public Button button;
        private Knowledgeable.Knowledge knowledge;
        private const string AssignedName = "Assigned";
        private bool last;
        private static readonly int Assigned = Animator.StringToHash(AssignedName);
        public Effect onPressed;
        private const string RemovedName = "CurrentlyAssigned";
        private static readonly int Removed = Animator.StringToHash(RemovedName);

        public Knowledgeable.Knowledge Knowledge => knowledge;
        private void Start() {
            if (button) {
                button.onClick.AddListener(delegate
                {
                    onPressed.PlayIfPresent(this);
                });
            }
        }
#if UNITY_EDITOR
        protected override void OnValidate() {
            animator.EnsureHasParameter(AssignedName, AnimatorControllerParameterType.Trigger);
            animator.EnsureHasParameter(RemovedName, AnimatorControllerParameterType.Bool);
        }
#endif
        public void Setup(Knowledgeable.Knowledge obj) {
            if (obj == knowledge) {
                return;
            }
            name = $"KnowledgeView - {obj}";
            knowledge = obj;
            var success = KnowledgeDatabase.Instance.icons.TryGetValue(obj, out var sprite);
            if (success) {
                svg.sprite = sprite;
                if (last) {
                    animator.SetTrigger(Assigned);
                }
            }
            last = success;
            animator.SetBool(Removed, success);
            
        }
    }
}