using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
namespace GGJ.Master.UI {
    [ExecuteInEditMode]
    public class OffsetScrolling : MonoBehaviour {
        public Vector2 scrollSpeed;

        [Required]
        public Image image;
        private Vector2 savedOffset;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        private void Update() {
            if (image == null) {
                return;
            }
            var x = Mathf.Repeat(Time.time * scrollSpeed.x, 1);
            var y = Mathf.Repeat(Time.time * scrollSpeed.y, 1);
            var offset = new Vector2(x, y);
            image.material.SetTextureOffset(MainTex, offset);
        }
    }
}