using Sirenix.OdinInspector;
using UI;
using UnityEngine;
namespace GGJ.Master.UI.Altar {
    public class AltarView : AnimatedView {

        [AssetsOnly]
        public AltarKnowledgeView prefab;
        public RectTransform list;
        private void Start() {
            GameManager.Instance.onAvailableKnowledgeFound.AddListener(OnAvailableChanged);
        }
        private static void OnAvailableChanged() {
            var available = GameManager.Instance.AvailableKnowledge;
            for (var i = 0; i < sizeof(ushort) * 8; i++) {
                var candidate = (Knowledgeable.Knowledge)(1 << i);
                if ((available & candidate) == candidate) {
                    // Unlocked
                    
                }
            }
        }
    }
}