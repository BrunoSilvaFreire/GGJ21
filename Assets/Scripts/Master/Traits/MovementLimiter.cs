using GGJ.Master;
using GGJ.Traits.Knowledge;
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
            knowledgeable.Bind(Knowledge.Knowledge.MoveLeft, OnMoveChanged);
            knowledgeable.Bind(Knowledge.Knowledge.MoveRight, OnMoveChanged);
            knowledgeable.Bind(Knowledge.Knowledge.Jump, OnJumpChanged);
        }
        private void OnJumpChanged(bool hasJump) {
            if (input.source == null) {
                input.source = Player.Instance.playerSource;
            }
            if (input.source is TransformableInputSource source) {
                if (!hasJump) {
                    source.jump += LimitJump;
                } else {
                    source.jump -= LimitJump;
                }    
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

            if (!knowledgeable.Matches(Knowledge.Knowledge.MoveLeft) && !knowledgeable.Matches(Knowledge.Knowledge.MoveRight)) {
                source.horizontal = LimitAll;
            } else {
                if (!knowledgeable.Matches(Knowledge.Knowledge.MoveLeft)) {
                    source.horizontal = LimitLeft;
                }
                if (!knowledgeable.Matches(Knowledge.Knowledge.MoveRight)) {
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