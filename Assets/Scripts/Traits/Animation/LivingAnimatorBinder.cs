using GGJ.Traits.Combat;
using Lunari.Tsuki.Entities;
using UnityEngine;
namespace GGJ.Traits.Animation {
    public class LivingAnimatorBinder : Trait {
        private Living living;
        private AnimatorBinder binder;
        public override void Configure(TraitDependencies dependencies) {
            dependencies.RequiresAnimatorParameter("Alive", AnimatorControllerParameterType.Bool);
            if (dependencies.DependsOn(out living, out binder)) {
                binder.BindBool("Alive", () => living.Alive);
            }
        }
    }
}