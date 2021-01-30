using System;
using Common;
using GGJ.Traits;
using Movement;
using Movement.States;
using Sirenix.OdinInspector;
using UnityEngine;
namespace GGJ.Master.Movements {
    public class SuperJumpAttachment : MotorState {
        [Required]
        public LookTilt tilt;
        public GroundedState state;
        [Required]
        public Animator animator;
        [NonSerialized]
        public float currentCharge;
        public float chargeSpeed;
        public float extraMultiplier;
        private MultiplierHandle handle;
        private bool sj;
        public override void Begin(Motor motor) {
            handle = state.jumpHeight.AddMultiplier(extraMultiplier);
        }
        public override void End(Motor motor) {
            state.jumpHeight.RemoveMultiplier(handle);
        }
        public override void Tick(Motor motor) {

            if (handle == null) {
                return;
            }

            if (currentCharge > 0.05F && motor.entityInput.jump.Continuous.JustActivated) {
                animator.SetTrigger("SuperJump");
            }
            if (tilt.lookingDown.Current) {
                if (currentCharge >= 1) {
                    currentCharge = 1;
                } else {
                    currentCharge += chargeSpeed * Time.fixedDeltaTime;
                }
            } else {
                currentCharge = 0;
            }
            handle.value = 1 + extraMultiplier * currentCharge;
        }
    }
}