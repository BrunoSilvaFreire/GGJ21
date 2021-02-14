using GGJ.Traits.Combat;
using Lunari.Tsuki.Entities;
using Movement;
namespace GGJ.Traits {
    public class StopOnDeath : Trait {
        public override void Configure(TraitDependencies dependencies) {
            if (dependencies.DependsOn(out Living living, out Motor motor)) {
                living.onDeath.AddListener(argfe0 => motor.maxSpeed.baseValue = 0);
            }
        }
    }
}