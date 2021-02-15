using System;
using UnityEngine;
using UnityEngine.UI;
namespace GGJ.UI {
    public class DelegateLayoutElement : MonoBehaviour, ILayoutElement {
        public Component other;
        public float padding;

        private void OnValidate() {
            if (other != null && !(other is ILayoutElement)) {
                other = other.gameObject.GetComponent<ILayoutElement>() as Component;
            }
        }

        private void Awake() {
            if (other != null) {
                 var rebuilder = other.gameObject.AddComponent<DelegateRebuilder>();
                rebuilder.onRebuild.AddListener(Rebuild);
            }
        }

        private void Rebuild() {
            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }

        public void CalculateLayoutInputHorizontal() {
            if (other is HorizontalOrVerticalLayoutGroup g) {
                g.CalculateLayoutInputHorizontal();
            }
        }

        public void CalculateLayoutInputVertical() {
            if (other is HorizontalOrVerticalLayoutGroup g) {
                g.CalculateLayoutInputVertical();
            }
        }

        private T DelegateOrDefault<T>(Func<ILayoutElement, T> func) {
            if (other is ILayoutElement e) {
                return func(e);
            }

            return default(T);
        }

        public float minWidth => DelegateOrDefault(element => element.minHeight) + padding * 2;


        public virtual float preferredWidth => DelegateOrDefault(element => element.preferredWidth) + padding * 2;

        public float flexibleWidth => DelegateOrDefault(element => element.flexibleHeight);

        public float minHeight => DelegateOrDefault(element => element.minHeight) + padding;

        public virtual float preferredHeight => DelegateOrDefault(element => element.preferredHeight) + padding;

        public float flexibleHeight => DelegateOrDefault(element => element.flexibleHeight);

        public int layoutPriority => DelegateOrDefault(element => element.layoutPriority);
    }
}