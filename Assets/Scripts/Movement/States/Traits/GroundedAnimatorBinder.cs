using GGJ.Traits.Animation;
using Lunari.Tsuki.Entities;
using UnityEngine;
namespace GGJ.Movement.States.Traits {
    [TraitLocation(TraitLocations.View)]
    public class GroundedAnimatorBinder : Trait {
        private Motor motor;

        public string speedPercentKey = "SpeedPercent";
        public string speedRawKey = "SpeedRaw";
        public string groundedKey = "Grounded";
        public string ySpeedKey = "YSpeed";
        public string absXInput = "absXInput";
        public override void Configure(TraitDependencies dependencies) {
            dependencies.DependsOn(out motor);
            if (dependencies.DependsOn(out AnimatorBinder binder)) {
                binder.BindFloat(absXInput, () => Mathf.Abs(motor.entityInput.horizontal));
                binder.BindFloat(ySpeedKey, () => motor.rigidbody.velocity.y);
                binder.BindFloat(speedRawKey, () => motor.rigidbody.velocity.x);
                binder.BindFloat(speedPercentKey, () => motor.rigidbody.velocity.magnitude / motor.maxSpeed);
                binder.BindBool(groundedKey, () => motor.supportState.down);
            }
            dependencies.RequiresAnimatorParameter(speedPercentKey, AnimatorControllerParameterType.Float);
            dependencies.RequiresAnimatorParameter(speedRawKey, AnimatorControllerParameterType.Float);
            dependencies.RequiresAnimatorParameter(groundedKey, AnimatorControllerParameterType.Bool);
            dependencies.RequiresAnimatorParameter(ySpeedKey, AnimatorControllerParameterType.Float);
            dependencies.RequiresAnimatorParameter(absXInput, AnimatorControllerParameterType.Float);
        }
    }
}