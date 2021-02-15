using GGJ.Props.Activatables;
using UnityEngine;
#if UNITY_EDITOR

#endif
namespace GGJ.Props.Common {
    public class Transporter : Activatable {
        public Vector2 posA, posB;

        [SerializeField]
        private float position;

        public float speed = 0.1F;
        public AnimationCurve velocity;

        private void FixedUpdate() {
            Position = Mathf.Lerp(position, @on ? 1 : 0, speed * Time.fixedDeltaTime);
        }

        public float Position {
            get => position;
            set {
                position = value;
                OnValidate();
            }
        }

        private void OnDrawGizmos() {
            Gizmos.DrawLine(posA, posB);
        }

        private void OnValidate() {
            transform.position = Vector2.Lerp(posA, posB, velocity.Evaluate(position));
        }
    }
}