using System;
using GGJ.Game;
using GGJ.Game.Traits;
using GGJ.Movement;
using GGJ.Traits.Combat;
using Lunari.Tsuki.Entities;
using UnityEngine;
namespace GGJ.Master.Movements {
    public class RollState : MotorState {
        public AnimationCurve rollCurve;
        public float rollDuration, rollSpeed;
        public float rollCooldown = 1;
        public MotorState toReturn;

        private float currentTime;
        private int direction;
        private float currentCooldown;
        private float oldScale;
        private void Update() {
            if (currentCooldown > 0) {
                currentCooldown -= Time.deltaTime;
            }
        }
        public override bool CanTransitionInto(Motor motor) {
            return currentCooldown <= 0;
        }
        public override void Begin(Motor motor) {
            TryMakeInvincible(motor, true);
            currentTime = 0;
            direction = Math.Sign(motor.entityInput.horizontal);
            oldScale = motor.rigidbody.gravityScale;
            motor.rigidbody.gravityScale = 0;
        }
        public override void Tick(Motor motor) {
            var vel = motor.rigidbody.velocity;
            vel.y = 0;
            var control = motor.GetDirectionControl(direction);
            vel.x = rollCurve.Evaluate(currentTime / rollDuration) * rollSpeed * control * direction;
            currentTime += Time.unscaledDeltaTime;
            motor.rigidbody.velocity = vel;
            if (currentTime >= rollDuration) {
                motor.ActiveState = toReturn;
            }
        }
        private void TryMakeInvincible(Motor motor, bool invincible) {
            if (motor.Owner.Access(out Knowledgeable knowledgeable)) {
                if (motor.Owner.Access(out Living living)) {
                    if (knowledgeable.Matches(Knowledge.Dodge)) {
                        living.invincible = invincible;
                    }
                }
            }
        }
        public override void End(Motor motor) {
            TryMakeInvincible(motor, false);
            currentCooldown = rollCooldown;
            motor.rigidbody.velocity = Vector2.zero;
            motor.rigidbody.gravityScale = oldScale;
        }
    }
}