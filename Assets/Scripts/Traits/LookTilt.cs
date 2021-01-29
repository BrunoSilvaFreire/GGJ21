using Cinemachine;
using GGJ.Master;
using GGJ.Traits.Knowledge;
using Input;
using Lunari.Tsuki.Entities;
using Movement;
using Unity.Mathematics;
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
        private bool lookingUp, lookingDown;
        private Motor motor;
        public override void Configure(TraitDependencies dependencies) {
            if (dependencies.DependsOn(out filmed, out Knowledgeable knowledgeable, out input, out motor)) {
                knowledgeable.Bind(Knowledgeable.Knowledge.LookUp, value => canLookUp = value);
                knowledgeable.Bind(Knowledgeable.Knowledge.LookDown, value => canLookDown = value);
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
                lookingUp = true;
                target += amount * v;
            } else {
                lookingUp = false;
            }
            if (canLookDown && v < 0) {
                lookingDown = true;
                target -= amount * math.abs(v);
            } else {
                lookingDown = false;
            }

            if (lookingUp || lookingDown) {
                motor.Control = 0;
            } else {
                motor.Control = 1;
            }
            framingTransposer.m_ScreenY = math.lerp(framingTransposer.m_ScreenY, target, updateSpeed * Time.deltaTime);
        }
    }
}