using Lunari.Tsuki.Entities;
using Movement.States;
using Sirenix.OdinInspector;
using UnityEngine;
namespace GGJ.Traits.Animation {
    [TraitLocation("Movement/Glide")]
    public class GlideAnimatorBinder : Trait {
        private GlideAttachment glide;
        public override void Configure(TraitDependencies dependencies) {
            glide = dependencies.RequiresComponent<GlideAttachment>("Movement/Glide");
            dependencies.RequiresAnimatorParameter("Gliding", AnimatorControllerParameterType.Bool);
            if (dependencies.DependsOn(out AnimatorBinder binder)) {
                binder.BindBool("Gliding", () => glide.gliding);
            }
        }
    }
}