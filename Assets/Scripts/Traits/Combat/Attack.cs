using Common;
using UnityEngine;
namespace GGJ.Traits.Combat {
    public abstract class Attack : MonoBehaviour, ISetupable<Combatant> {
        protected Combatant combatant;
        public void Setup(Combatant obj) {
            combatant = obj;
        }


        public abstract void Execute(Combatant combatant);
    }
}