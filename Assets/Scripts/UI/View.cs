using Lunari.Tsuki.Runtime.Stacking;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
#endif

namespace GGJ.UI {
    public abstract class View : MonoBehaviour {
        public const string ViewGroup = "View Stuff";

        public UnityEvent onShow;
        public UnityEvent onHide;

        [SerializeField, LabelText("Internal Shown")]
        protected BooleanStackable shown;
        public void Show() {
            Show(false);
        }
        public void Show(bool immediate) {
            if (shown) {
                return;
            }
            shown = true;
            if (immediate) {
                ImmediateReveal();
            } else {
                Reveal();
            }

            onShow.Invoke();
        }
        public void Hide() {
            Hide(false);
        }
        public void Hide(bool immediate) {
            if (!shown) {
                return;
            }
            shown = false;
            if (immediate) {
                ImmediateConceal();
            } else {
                Conceal();
            }
            onHide.Invoke();
        }

        public Modifier<bool> AddShownModifier(bool value) {
            return shown.AddModifier(value);
        }

        public void RemoveShownModifier(Modifier<bool> handle) {
            shown.RemoveModifier(handle);
        }

        [ShowInInspector, BoxGroup(ViewGroup)]
        public bool Shown {
            get => shown;
            set {
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying) {
                    if (value) {
                        Show(true);
                    } else {
                        Hide(true);
                    }
                    return;
                }
#endif
                if (value) {
                    Show();
                } else {
                    Hide();
                }
            }
        }

        protected abstract void Conceal();
        protected abstract void Reveal();
        protected abstract void ImmediateConceal();
        protected abstract void ImmediateReveal();
        public abstract bool IsFullyShown();

    }
}