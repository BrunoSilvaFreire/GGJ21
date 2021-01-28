using Cinemachine;
using GGJ.Master;
using GGJ.Traits.Knowledge;
using Input;
using Lunari.Tsuki.Entities;
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
        public override void Configure(TraitDependencies dependencies) {
            if (dependencies.DependsOn(out filmed, out Knowledgeable knowledgeable, out input)) {
                knowledgeable.Bind(Knowledgeable.Knowledge.LookUp, value => canLookUp = value);
                knowledgeable.Bind(Knowledgeable.Knowledge.LookDown, value => canLookDown = value);
                framingTransposer = filmed.Camera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }
        private void Update() {
            if (input == null) {
                return;
            }
            var target = 0.5F;
            var v = input.vertical;
            if (canLookUp && v > 0) {
                target += amount * v;
            }
            if (canLookDown && v < 0) {
                target -= amount * math.abs(v);
            }

            framingTransposer.m_ScreenY = math.lerp(framingTransposer.m_ScreenY, target, updateSpeed * Time.deltaTime);
        }
    }
}