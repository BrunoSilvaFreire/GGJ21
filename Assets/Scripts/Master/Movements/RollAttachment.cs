using GGJ.Traits;
using GGJ.Traits.Combat;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Entities;
using Movement;
using UnityEngine;
namespace GGJ.Master.Movements {
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