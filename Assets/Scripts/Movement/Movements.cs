using System;
using UnityEngine;
namespace GGJ.Movement {
    public static partial class Movements {
        public static bool Jumping(
            Motor motor,
            float jumpHeight,
            float extraGravity
        ) {
            var rigidbody = motor.rigidbody;
            var input = motor.entityInput;
            var velocity = rigidbody.velocity;
            var jumped = motor.entityInput.jump.Discrete.Consume();
            if (jumped && motor.IsJumpEligible()) {
                velocity.y = jumpHeight;
            }

            if (!motor.supportState.down && !input.jump.Current && velocity.y > 0) {
                velocity.y -= extraGravity;
            }

            rigidbody.velocity = velocity;
            return jumped;
        }


        public static void Horizontal(
            Motor motor,
            float deceleration,
            float speed
        ) {
            var rigidbody = motor.rigidbody;
            var input = motor.entityInput;
            var velocity = rigidbody.velocity;
            var horizontal = input.horizontal;
            var inputDir = Math.Sign(horizontal);
            var control = motor.GetDirectionControl(inputDir);
            var max = motor.maxSpeed * control * Mathf.Abs(horizontal);
            
            if (Mathf.Approximately(horizontal, 0)) {
                // Slowly stop
                velocity.x = Mathf.Lerp(velocity.x, 0, deceleration);
            } else {
                var untilMaxSpeed = Mathf.Max(0, max - Mathf.Abs(velocity.x));
                var wantsToAdd = speed * control;
                var velDir = Mathf.Sign(velocity.x);
                if (Mathf.Approximately(velDir, inputDir)) {
                    // Accelerate
                    velocity.x += inputDir * Mathf.Min(untilMaxSpeed, wantsToAdd);
                } else {
                    // Decelerate
                    velocity.x += inputDir * Mathf.Min(max, wantsToAdd);
                }
                if (motor.supportState.down) {
                    velocity.x = Mathf.Clamp(velocity.x, -max, max);
                }
            }

            rigidbody.velocity = velocity;
        }

        public static bool IsJumpEligible(this Motor entity) {
            return entity.supportState.down;
        }
    }
}