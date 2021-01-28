using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
namespace UI {
    public class TransparentView : View {
        public const string TransparencyGroup = "Transparency & Transition Stuff";
        public bool alsoBlockInteraction = true;
        [Required, BoxGroup(TransparencyGroup)]
        public CanvasGroup canvasGroup;

        [BoxGroup(TransparencyGroup)]
        public float revealedOpacity = 1, concealedOpacity;

        [BoxGroup(TransparencyGroup)]
        public float revelationTransitionDuration = 0.5F, concealTransitionDuration;

        protected override void Conceal() {
            AnimateTo(concealedOpacity, concealTransitionDuration);
            TrySetBlocked(true);
        }


        protected override void Reveal() {
            AnimateTo(revealedOpacity, revelationTransitionDuration);
            TrySetBlocked(false);
        }
        private void TrySetBlocked(bool b) {
            if (alsoBlockInteraction) {
                canvasGroup.interactable = !b;
            }
        }

        private void AnimateTo(float target, float duration) {
            canvasGroup.DOKill();
            canvasGroup.DOFade(target, duration);
        }

        protected override void ImmediateConceal() {
            canvasGroup.DOKill();
            canvasGroup.alpha = concealedOpacity;
            TrySetBlocked(true);
        }

        protected override void ImmediateReveal() {
            canvasGroup.DOKill();
            canvasGroup.alpha = revealedOpacity;
            TrySetBlocked(false);
        }

        public override bool IsFullyShown() {
            return Mathf.Abs(revealedOpacity - canvasGroup.alpha) < 0.05;
        }
    }
}