using System.Linq;
using UnityEngine;
namespace UI {
    public class CompositeView : View {
        public View[] subviews;

        private void DebugInvalidView() {
            Debug.Log($"Found invalid subview in {gameObject.name}", this);
        }

        protected override void Conceal() {
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

        protected override void Reveal() {
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
}