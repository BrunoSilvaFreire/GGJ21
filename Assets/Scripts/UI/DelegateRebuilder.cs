using UnityEngine;
using UnityEngine.Events;
namespace GGJ.UI {
    public class DelegateRebuilder : MonoBehaviour {
        public UnityEvent onRebuild = new UnityEvent();

        private void OnRectTransformDimensionsChange() {
            onRebuild.Invoke();
        }
    }
}