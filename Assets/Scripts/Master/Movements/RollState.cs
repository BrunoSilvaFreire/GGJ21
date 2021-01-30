using System;
using Movement;
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
        private void Update() {
            if (currentCooldown > 0) {
                currentCooldown -= Time.deltaTime;
            }
        }
        public override bool CanTransitionInto(Motor motor) {
            return currentCooldown <= 0;
        }
        public override void Begin(Motor motor) {
            currentTime = 0;
            direction = Math.Sign(motor.entityInput.horizontal);
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
        public override void End(Motor motor) {
            currentCooldown = rollCooldown;
        }
    }
}