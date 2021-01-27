using System;
using Input;
using UnityEngine;
namespace Movement.States {
    public class FlyingState : MotorState {
        private float lastScale;
        public float deaccelerationAmount = 5;

        public AnimationCurve accelerationCurve = AnimationCurve.EaseInOut(
            0,
            1,
            1,
            8
        );

        public float opposingForceMultiplier = 2;

        public override void Begin(Motor motor) {
            lastScale = motor.rigidbody.gravityScale;
            motor.rigidbody.gravityScale = 0;
        }

        private void Move(
            float input,
            ref float speed,
            float maxSpeed
        ) {
            var inputDir = Math.Sign(input);
            var velDir = Math.Sign(speed);

            var speedPercent = speed / maxSpeed;
            var rawAcceleration = accelerationCurve.Evaluate(speedPercent);
            var acceleration = rawAcceleration * inputDir;

            //Check acceleration if (is stopped or (input is not empty and input is same dir as vel))
            if (velDir == 0 || velDir == inputDir && inputDir != 0) {
                speed += acceleration;
            } else {
                var deceleration = accelerationCurve.Evaluate(1 - speedPercent);
                if (Mathf.Abs(input) > 0) {
                    var d = deceleration * inputDir;
                    if (inputDir != 0 && velDir != 0) {
                        d *= opposingForceMultiplier;
                    }

                    speed += d;
                } else {
                    //Not inputting
                    if (speed < rawAcceleration) {
                        speed = 0;
                    } else {
                        speed += deceleration * -velDir;
                    }
                }
            }
        }

        public override void Tick(Motor motor) {
            var body = motor.rigidbody;
            var provider = motor.Owner.GetTrait<EntityInput>();
            var hasProvider = provider != null;
            var xInput = hasProvider && !provider.locked ? provider.horizontal : 0;
            var yInput = hasProvider && !provider.locked ? provider.vertical : 0;

            var vel = body.velocity;
            Move(xInput, ref vel.x, motor.maxSpeed);
            Move(yInput, ref vel.y, motor.maxSpeed);
            body.velocity = Vector2.ClampMagnitude(vel, motor.maxSpeed);
        }

        public override void End(Motor motor) {
            motor.rigidbody.gravityScale = lastScale;
        }
    }
}