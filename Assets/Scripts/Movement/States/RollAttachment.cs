using GGJ.Traits;
using UnityEngine;
namespace GGJ.Movement.States {
    public class RollAttachment : MotorState {
        public LookTilt look;
        public RollState roll;
  
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
    }
}