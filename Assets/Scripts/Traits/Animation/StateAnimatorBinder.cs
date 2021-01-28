using Lunari.Tsuki.Entities;
using Movement;
using UnityEngine;
namespace GGJ.Traits.Animation {
    [TraitLocation("Movement")]
    public class StateAnimatorBinder : Trait {
        public MotorState[] states;

        public override void Configure(TraitDependencies dependencies) {
            if (states != null) {
                foreach (var state in states) {
                    dependencies.RequiresAnimatorParameter(state.name, AnimatorControllerParameterType.Bool);
                }
            }
            if (dependencies.DependsOn(out Motor motor, out AnimatorBinder binder)) {
                foreach (var state in states) {
                    binder.BindBool(state.name, () => motor.ActiveState == state);
                }
            }
        }
    }
}