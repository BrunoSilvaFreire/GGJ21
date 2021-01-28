using System;
using Lunari.Tsuki.Entities;
using Movement;
using UnityEngine;
namespace GGJ.Traits {
    public class SpriteFlipper : Trait {
        public new Rigidbody2D rigidbody;
        public new SpriteRenderer renderer;
        public bool facesRight;
        private Motor motor;
        public override void Configure(TraitDependencies dependencies) {
            dependencies.DependsOn(out motor);
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