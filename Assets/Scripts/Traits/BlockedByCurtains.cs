using GGJ.Master.UI;
using Input;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime.Stacking;
namespace GGJ.Traits {
    public class BlockedByCurtains : Trait {
        private Modifier<float> horizontalModifier;
        private Modifier<float> verticalModifier;
        public override void Configure(TraitDependencies dependencies) {
            if (dependencies.DependsOn(out EntityInput input)) {
                var curtains = PlayerUI.Instance.deathCurtains;
                curtains.onShow.AddListener(delegate {
                    horizontalModifier = input.horizontal.AddModifier(0);
                    verticalModifier = input.vertical.AddModifier(0);
                    input.jump.overriden = true;
                    input.jump.overwriteValue = false;
                });
                curtains.onHide.AddListener(delegate {
                    if (horizontalModifier != null) {
                        input.horizontal.RemoveModifier(horizontalModifier);
                    }
                    if (verticalModifier != null) {
                        input.vertical.RemoveModifier(verticalModifier);
                    }
                    input.jump.overriden = false;
                });
            }
        }
    }
}