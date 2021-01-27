using Common;
using UnityEngine;
namespace Movement {
    public class RaycastUpdater : SupportStateUpdater {
        public float length = 0.1F;
        private LayerMask layerMask;

        private void Start() {
            layerMask = GameConfiguration.Instance.worldMask;
        }

        private bool Check(Vector2 origin, Vector2 direction) {
            var results = new RaycastHit2D[1];
            Physics2D.queriesHitTriggers = false;
            var l = Physics2D.LinecastNonAlloc(origin, origin + (direction * length), results, layerMask);
            Physics2D.queriesHitTriggers = true;
            /*if (l > 0) {
                foreach (var hit2D in results) {
                    Debug.Log(hit2D.collider);
                }
            }*/
            return l > 0;
        }

        public override void Test(Motor motor, ref SupportState state) {
            var bounds = motor.collider.bounds;
            var center = bounds.center;
            var min = bounds.min;
            var max = bounds.max;
            state.up = Check(new Vector2(center.x, max.y), Vector2.up);
            state.down = Check(new Vector2(center.x, min.y), Vector2.down);
            state.left = Check(new Vector2(min.x, center.y), Vector2.left);
            state.right = Check(new Vector2(max.x, center.y), Vector2.right);
        }
    }
}