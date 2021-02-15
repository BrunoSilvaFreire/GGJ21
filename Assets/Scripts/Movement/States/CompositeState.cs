using System;
using System.Collections.Generic;
namespace GGJ.Movement.States {
    public class CompositeState : MotorState {
        public List<MotorState> children;

        // ReSharper disable once SuggestBaseTypeForParameter
        private static void TryInvoke(MotorState motorizer, Motor motor, Action<Motor> action) {
            if (motorizer.enabled) {
                action(motor);
            }
        }

        public override void Begin(Motor motor) {
            foreach (var motorizer in children) {
                TryInvoke(motorizer, motor, motorizer.Begin);
            }
        }

        public override void Tick(Motor motor) {
            foreach (var motorizer in children) {
                TryInvoke(motorizer, motor, motorizer.Tick);
            }
        }

        public override void End(Motor motor) {
            foreach (var motorizer in children) {
                TryInvoke(motorizer, motor, motorizer.End);

            }
        }
    }
}