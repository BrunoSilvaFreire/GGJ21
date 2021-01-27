using GGJ.Master;
using Input;
using Lunari.Tsuki.Entities;
using UnityEngine;
namespace GGJ.Traits {
    [TraitLocation("Misc")]
    public class MovementLimiter : Trait {
        private Knowledgeable knowledgeable;
        private EntityInput input;

        public override void Configure(TraitDependencies dependencies) {
            if (!dependencies.DependsOn(out knowledgeable, out input)) {
                return;
            }
            knowledgeable.Bind(Knowledgeable.Knowledge.MoveLeft, OnMoveChanged);
            knowledgeable.Bind(Knowledgeable.Knowledge.MoveRight, OnMoveChanged);
            knowledgeable.Bind(Knowledgeable.Knowledge.Jump, OnJumpChanged);
        }
        private void OnJumpChanged(bool arg0) {
            if (!(input.source is TransformableInputSource source)) {
                return;
            }
            if (!arg0) {
                source.jump += LimitJump;
            } else {
                source.jump -= LimitJump;
            }
        }
        private static bool LimitJump(bool value) => false;
        private void OnMoveChanged(bool _) {
            OnMoveChanged();
        }
        private void OnMoveChanged() {
            if (!(input.source is TransformableInputSource source)) {
                return;
            }
            source.horizontal = null;

            if (!knowledgeable.Matches(Knowledgeable.Knowledge.MoveLeft) && !knowledgeable.Matches(Knowledgeable.Knowledge.MoveRight)) {
                source.horizontal = LimitAll;
            } else {
                if (!knowledgeable.Matches(Knowledgeable.Knowledge.MoveLeft)) {
                    source.horizontal = LimitLeft;
                }
                if (!knowledgeable.Matches(Knowledgeable.Knowledge.MoveRight)) {
                    source.horizontal = LimitRight;
                }
            }

        }
        private float LimitLeft(float value) {
            return Mathf.Max(0, value);
        }

        private float LimitRight(float value) {
            return Mathf.Min(0, value);
        }
        private float LimitAll(float value) {
            return 0;
        }
    }
}