using UnityEngine;
namespace GGJ.World.Legacy {
    [RequireComponent(typeof(Sprite))]
    public class TiledSpriteRenderer : MonoBehaviour, ITiledObject {

        private SpriteRenderer m_renderer;
        public void Setup(ObjectData data) {
            m_renderer = GetComponent<SpriteRenderer>();
            
        }
    }
}