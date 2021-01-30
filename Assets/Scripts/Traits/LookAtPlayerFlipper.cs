using System;
using GGJ.Master;
using Lunari.Tsuki.Entities;
using Movement;
using UnityEngine;
namespace GGJ.Traits {
    [TraitLocation(TraitLocations.View)]
    public class LookAtPlayerFlipper : Trait {
        private Entity target;
        private SpriteRenderer spriteRenderer;
        public bool facesRight;
        public override void Configure(TraitDependencies dependencies) {
            spriteRenderer = dependencies.RequiresComponent<SpriteRenderer>(TraitLocations.View);
        }
        private void Start() {
            target = Player.Instance.Pawn;
            Player.Instance.onPawnChanged.AddListener(arg0 => target = arg0);
        }
        private void Update() {
            if (target != null) {
                var dir = (HorizontalDirection)Math.Sign(transform.position.x - target.transform.position.x);
                switch (dir) {
                    case HorizontalDirection.Left:
                        spriteRenderer.flipX = facesRight;
                        break;

                    case HorizontalDirection.Right:
                        spriteRenderer.flipX = !facesRight;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}