using Lunari.Tsuki.Entities;
using UnityEngine;
namespace Input {
    public class AlignWithAim : Trait {
        public Transform target;
        private EntityInput input;
        public override void Configure(TraitDependencies dependencies) {
            dependencies.DependsOn(out input);
        }
        private void Start() {
            if (target == null) {
                target = transform;
            }
        }
        private void Update() {
            if (target == null) {
                return;
            }
            var aim = input.aim;
            var angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
            target.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}