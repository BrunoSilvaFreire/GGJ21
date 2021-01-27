using System;
using System.Collections.Generic;
namespace Movement.States {
    public class CompositeState : MotorState {
        public List<MotorState> children;

        private bool DelegateTo(Action<Motor> del, Motor motor) {
            del(motor);
            return motor.ActiveState == this;
        }

        public override void Begin(Motor motor) {
            foreach (var motorizer in children) {
                if (!DelegateTo(motorizer.Begin, motor)) {
                    //break;
                }
            }
        }

        public override void Tick(Motor motor) {
            foreach (var motorizer in children) {
                if (!DelegateTo(motor1 => motorizer.Tick(motor1), motor)) {
                    //break;
                }
            }
        }

        public override void End(Motor motor) {
            foreach (var motorizer in children) {
                if (!DelegateTo(motorizer.End, motor)) {
                    //break;
                }
            }
        }
    }
}