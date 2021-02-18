using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
namespace GGJ.UI.Common {
    public class UIRebuilder : MonoBehaviour {
        [ShowInInspector]
        public void Rebuild() {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
        }
    }
}