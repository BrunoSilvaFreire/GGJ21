using System.Linq;
using UnityEngine;
namespace UI {
    public class AnimatedView : View {
        public Animator animator;
        public string revealedState = "Revealed";
        public string concealedState = "Concealed";
        private static readonly int RevealKey = Animator.StringToHash("Visible");
#if UNITY_EDITOR
        protected virtual void OnValidate() {
            if (animator != null) {
                if (animator.runtimeAnimatorController is UnityEditor.Animations.AnimatorController controller) {
                    if (controller.parameters.Any(parameter => parameter.nameHash == RevealKey)) {
                        return;
                    }
                    controller.AddParameter("Visible", AnimatorControllerParameterType.Bool);
                }
            }
        }
#endif

        protected override void Conceal() {
            animator.SetBool(RevealKey, false);
        }

        protected override void Reveal() {
            animator.SetBool(RevealKey, true);
        }

        protected override void ImmediateConceal() {
            Conceal();
            animator.Play(concealedState, 0, 1);
        }

        protected override void ImmediateReveal() {
            Reveal();
            animator.Play(revealedState, 0, 1);
        }

        public override bool IsFullyShown() {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            return state.normalizedTime > 0.95;
        }
    }
}