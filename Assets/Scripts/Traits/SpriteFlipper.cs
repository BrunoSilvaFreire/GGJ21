using Lunari.Tsuki.Entities;
using UnityEngine;
namespace GGJ.Traits {
    public class SpriteFlipper : Trait {
        public new Rigidbody2D rigidbody;
        public new SpriteRenderer renderer;
        public bool facesRight;

        private void Update() {
            UpdateTo(rigidbody.velocity.x);
        }

        public void UpdateTo(float dir) {
            if (dir > 0) {
                renderer.flipX = facesRight;
            }

            if (dir < 0) {
                renderer.flipX = !facesRight;
            }
        }
    }
}