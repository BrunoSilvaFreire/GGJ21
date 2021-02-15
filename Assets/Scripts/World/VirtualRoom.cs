using Cinemachine;
using GGJ.Game;
using GGJ.World.Legacy;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.World {
    public class VirtualRoom : Trait {
        public UnityEvent onActivated, onDeactivated;

        [MinMaxSlider(0, 20, ShowFields = true)]
        public Vector2Int priority;

        public new CinemachineVirtualCamera camera;

        private new BoxCollider2D collider;

        private static bool Allowed(Collider2D col) {
            var e = col.GetComponentInParent<Entity>();
            if (e == null) {
                return false;
            }

            return e == Player.Instance.Pawn && col.gameObject == e.gameObject;
        }

#if UNITY_EDITOR
        private void OnValidate() {
            if (collider) {
                collider.isTrigger = true;
            }
        }
#endif

        private void OnDrawGizmos() {
            if (collider != null) {
                Gizmos2.DrawBounds2D(collider.bounds, Color.magenta);
            }
        }

        private void Awake() {
            if (camera == null) {
                return;
            }
            camera.Priority = priority.x;
            if (this.TryGetComponentInParent(out Map map)) {
                camera.name = $"Room Camera - {map.Coordinates}";
            } else {
                camera.name = "Room Camera - Unknown";
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (Allowed(other)) {
                onActivated.Invoke();
                if (camera != null) {
                    camera.Priority = priority.y;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (Allowed(other)) {
                onDeactivated.Invoke();
                if (camera != null) {
                    camera.Priority = priority.x;
                }
            }
        }
    }
}