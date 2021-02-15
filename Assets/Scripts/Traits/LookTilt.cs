using Cinemachine;
using GGJ.Common;
using GGJ.Game;
using GGJ.Game.Traits;
using GGJ.Input;
using GGJ.Movement;
using GGJ.Traits.Animation;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime.Stacking;
using UnityEngine;

namespace GGJ.Traits {
    [TraitLocation("Misc")]
    public class LookTilt : Trait {
        public float amount = 0.25F;
        public float updateSpeed = 5F;
        private Filmed filmed;
        private EntityInput input;
        private bool canLookUp, canLookDown;
        private CinemachineFramingTransposer framingTransposer;
        public BooleanHistoric lookingUp = new BooleanHistoric();
        public BooleanHistoric lookingDown = new BooleanHistoric();
        private Motor motor;
        public float crouchSpeedMultiplier = .25F;
        private Modifier<float> handle;
        public override void Configure(TraitDependencies dependencies) {
            if (dependencies.DependsOn(out filmed, out Knowledgeable knowledgeable, out input, out motor)) {
                knowledgeable.Bind(Knowledge.LookUp, value => canLookUp = value);
                knowledgeable.Bind(Knowledge.LookDown, value => canLookDown = value);
                framingTransposer = filmed.Camera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
            if (dependencies.Access(out AnimatorBinder binder)) {
                dependencies.RequiresAnimatorParameter("LookingUp", AnimatorControllerParameterType.Bool);
                dependencies.RequiresAnimatorParameter("Crouching", AnimatorControllerParameterType.Bool);
                if (dependencies.Initialize) {
                    binder.BindBool("LookingUp", () => lookingUp);
                    binder.BindBool("Crouching", () => lookingDown);
                }
            }
        }
        private void Update() {
            if (input == null) {
                return;
            }
            var target = 0.5F;
            var v = input.vertical;
            if (canLookUp && v > 0) {
                lookingUp.Current = true;
                target += amount * v;
            } else {
                lookingUp.Current = false;
            }
            if (canLookDown && v < 0) {
                lookingDown.Current = true;
                target -= amount * Mathf.Abs(v);
            } else {
                lookingDown.Current = false;
            }

            if (lookingDown.JustActivated) {
                handle = motor.maxSpeed.AddModifier(crouchSpeedMultiplier);
            }
            if (lookingDown.JustDeactivated) {
                if (handle != null) {
                    motor.maxSpeed.RemoveModifier(handle);
                }
            }

            framingTransposer.m_ScreenY = Mathf.Lerp(framingTransposer.m_ScreenY, target, updateSpeed * Time.deltaTime);
        }
    }
}