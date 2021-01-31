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
        public override void Begin(Motor motor) {
            if (motor.Owner.Access(out Knowledgeable knowledgeable)) {
                if (motor.Owner.Access(out Living living)) {
                    if (knowledgeable.Matches(Knowledgeable.Knowledge.Dodge)) {
                        living.invincible = true;
                    }
                }
            }
        }
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
        public override void End(Motor motor) {
            if (motor.Owner.Access(out Knowledgeable knowledgeable)) {
                if (motor.Owner.Access(out Living living)) {
                    if (knowledgeable.Matches(Knowledgeable.Knowledge.Dodge)) {
                        living.invincible = false;
                    }
                }
            }
        }
    }
}