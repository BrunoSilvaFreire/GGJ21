using System;
using Common;
using GGJ.Traits;
using Lunari.Tsuki.Runtime.Stacking;
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
        public float extraMultiplier;
        [ShowInInspector]
        private Modifier<float> handle;
        public override void Begin(Motor motor) {
        }
        public override void End(Motor motor) {
            state.jumpHeight.RemoveModifier(handle);
        }
        public override void Tick(Motor motor) {
            if (handle == null) {
                handle = state.jumpHeight.AddModifier(extraMultiplier);
                return;
            }

            float currentCharge;
            if (tilt.lookingUp.Current) {
                if (motor.entityInput.jump.Continuous.JustActivated) {
                    animator.SetTrigger("SuperJump");
                }
                currentCharge = 1;
            } else {
                currentCharge = 0;
            }
            handle.value = 1 + extraMultiplier * currentCharge;
        }
    }
}