using System;
using Movement;
using UnityEngine;
namespace GGJ.Master.Movements {
    public class RollState : MotorState {
        public AnimationCurve rollCurve;
        public float rollDuration, rollSpeed;
        private float currentTime = 0;
        private int direction;
        public MotorState toReturn;
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
        public override void End(Motor motor) { }
    }
}