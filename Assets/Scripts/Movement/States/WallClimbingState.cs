using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Input;
using Lunari.Tsuki.Entities;
using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using UnityEngine;
namespace Movement.States {
    public class WallClimbingState : MotorState {
        private float currentTime = 0;
        public float controlRegainCooldown = 1.5F;
        public float pushoffForce = 23;
        public float jumpForce = 23;
        public AnimationCurve slipCurve = AnimationCurve.EaseInOut(0, 0, 3, 10);
        private float lastTimescale;
        public float cooldown = 1;
        public MotorState toReturn;
        public Effect frictionParticle, frictionImpactEffect;
        public float frictionParticleDistance;
        private float travelledDistanceSinceLastParticle;
        private float whenLeft;
        private TweenerCore<float, float, FloatOptions> old;

        public override bool CanTransitionInto(Motor motor) {
            return Time.time - whenLeft > cooldown;
        }

        public override void Begin(Motor motor) {
            currentTime = 0;
            lastTimescale = motor.rigidbody.gravityScale;
            motor.rigidbody.gravityScale = 0;
            PlayEffect(frictionImpactEffect, motor);
        }

        public override void End(Motor motor) {
            motor.rigidbody.gravityScale = lastTimescale;
            whenLeft = Time.time;
            PlayEffect(frictionImpactEffect, motor);
        }

        private void PlayEffect(Effect effect, Motor motor) {
            var bounds = motor.collider.bounds;
            var horDir = motor.supportState.Horizontal;
            var pos = bounds.center + new Vector3(bounds.extents.x * horDir, 0, 0);
            effect.PlayIfPresent(
                motor,
                false,
                new PositionFeature(pos)
            );
        }

        public override void Tick(Motor motor) {
            var vel = motor.rigidbody.velocity;
            var horDir = motor.supportState.Horizontal;
            travelledDistanceSinceLastParticle += Mathf.Abs(vel.y) * Time.deltaTime;
            if (travelledDistanceSinceLastParticle > frictionParticleDistance) {
                travelledDistanceSinceLastParticle = 0;
                PlayEffect(frictionParticle, motor);
            }

            currentTime += Time.fixedDeltaTime;
            vel.y = -slipCurve.Evaluate(currentTime);
            vel.x = horDir;
            if (motor.Owner.Access<EntityInput>(out var input)) {
                var iDir = Math.Sign(input.horizontal);
                if (iDir != horDir) {
                    motor.ActiveState = toReturn;
                    return;
                }

                if (input.jump.Discrete.Consume()) {
                    vel.x = -horDir * pushoffForce;
                    vel.y = jumpForce;
                    old?.Complete();
                    switch (horDir) {
                        case -1:
                            old = motor.DoLeftControl(1, controlRegainCooldown);
                            break;
                        case 1:
                            old = motor.DoRightControl(1, controlRegainCooldown);
                            break;
                    }
                    motor.GetDirectionControlReference(horDir) = 0;
                    if (old != null) {
                        old.onComplete = delegate {
                            old = null;
                        };
                    }

                    motor.ActiveState = toReturn;
                }
            }

            motor.rigidbody.velocity = vel;
        }
    }
}