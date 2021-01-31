using Lunari.Tsuki.Entities;
using Movement;
using UnityEngine;

namespace GGJ.Traits {
    [TraitLocation(TraitLocations.View)]
    public class SpriteFlipper : Trait {
        private new SpriteRenderer renderer;
        public bool facesRight;
        private Motor motor;
        public override void Configure(TraitDependencies dependencies) {
            dependencies.DependsOn(out motor);
            renderer = dependencies.RequiresComponent<SpriteRenderer>("View");
        }

        private void Update() {
            switch (motor.GetDirection()) {
                case HorizontalDirection.Left:
                    renderer.flipX = facesRight;
                    break;
                case HorizontalDirection.Right:
                    renderer.flipX = !facesRight;
                    break;
            }
        }
    }
}