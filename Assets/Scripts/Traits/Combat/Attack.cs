
using Common;
namespace GGJ.Traits.Combat {
    public abstract class Attack : Setupable<Combatant> {
        protected Combatant combatant;
        public override void Setup(Combatant obj) {
            combatant = obj;
        }


        public abstract void Execute(Combatant combatant);
    }
}