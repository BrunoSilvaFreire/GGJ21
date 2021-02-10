using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace UI {
    public class AbstractCompositeView<T> : View where T : View {
        public List<T> subviews;
        public Animator animator;
        private static readonly int Visible = Animator.StringToHash("Visible");
        private void DebugInvalidView() {
            Debug.Log($"Found invalid subview in {gameObject.name}", this);
        }
        protected override void Conceal() {
            TrySetAnimator(false);
            
            foreach (var view in subviews) {
#if UNITY_EDITOR
                if (view == null) {
                    DebugInvalidView();
                    continue;
                }
#endif

                view.Hide();
            }
        }
        private void TrySetAnimator(bool b) {
            if (animator != null) {
                animator.SetBool(Visible, b);
            }
        }
        protected override void Reveal() {
            TrySetAnimator(true);
            foreach (var view in subviews) {
#if UNITY_EDITOR
                if (view == null) {
                    DebugInvalidView();
                    continue;
                }
#endif
                view.Show();
            }
        }
        protected override void ImmediateConceal() {
            TrySetAnimator(false);
            foreach (var view in subviews) {
#if UNITY_EDITOR
                if (view == null) {
                    DebugInvalidView();
                    continue;
                }
#endif
                view.Hide(true);
            }
        }
        protected override void ImmediateReveal() {
            TrySetAnimator(true);
            foreach (var view in subviews) {
#if UNITY_EDITOR
                if (view == null) {
                    DebugInvalidView();
                    continue;
                }
#endif
                view.Show(true);
            }
        }
        public override bool IsFullyShown() {
            return subviews.All(view => view.IsFullyShown());
        }
    }
    public class CompositeView : AbstractCompositeView<View> { }
}