using GGJ.Traits;
using Movement;
using UnityEngine;
namespace GGJ.Master.Movements {
    public class RollAttachment : MotorState {
        public LookTilt look;
        public RollState roll;
        public override void Begin(Motor motor) { }
        public override void Tick(Motor motor) {
            if (!motor.supportState.down) {
                return;
            }
            if (!look.lookingDown) {
                return;
            }
            var dir = motor.entityInput.horizontal;
            if (Mathf.Approximately(dir, 0)) {
                return;
            }
            motor.ActiveState = roll;
        }
        public override void End(Motor motor) { }
    }
}