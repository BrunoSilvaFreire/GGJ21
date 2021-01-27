using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
namespace UI {
    public class TransparentView : View {
        public const string TransparencyGroup = "Transparency & Transition Stuff";

        [Required, BoxGroup(TransparencyGroup)]
        public CanvasGroup canvasGroup;

        [BoxGroup(TransparencyGroup)]
        public float revealedOpacity = 1, concealedOpacity;

        [BoxGroup(TransparencyGroup)]
        public float revelationTransitionDuration = 0.5F, concealTransitionDuration;

        protected override void Conceal() {
            AnimateTo(concealedOpacity, concealTransitionDuration);
        }


        protected override void Reveal() {
            AnimateTo(revealedOpacity, revelationTransitionDuration);
        }

        private void AnimateTo(float target, float duration) {
            canvasGroup.DOKill();
            canvasGroup.DOFade(target, duration);
        }

        protected override void ImmediateConceal() {
            canvasGroup.DOKill();
            canvasGroup.alpha = concealedOpacity;
        }

        protected override void ImmediateReveal() {
            canvasGroup.DOKill();
            canvasGroup.alpha = revealedOpacity;
        }

        public override bool IsFullyShown() {
            return Mathf.Abs(revealedOpacity - canvasGroup.alpha) < 0.05;
        }
    }
}