using System;
using Common;
using UnityEngine;
namespace Movement {
    [Serializable]
    public class GoingDownFilter : Filter {
        public Motor motor;
        public override bool Allowed(Collider2D collider2D) {
            return motor.rigidbody.velocity.y < 0;
        }
    }
}