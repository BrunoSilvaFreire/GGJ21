using System;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Traits.Combat {
    [Serializable]
    public class UnityDamageEvent : UnityEvent<DamageEvent> {
    }

    [Serializable]
    public class DamageEvent {

        public uint HealthAfter => Math.Max(0, Target.Health - Value);

        public uint Value { get; set; }

        public DamageEvent(Damage damage, Living target) {
            Damage = damage;
            Target = target;
            Value = damage.Original;
        }

        public Damage Damage {
            get;
        }

        public Living Target {
            get;
        }
    }

    [Serializable]
    public struct Damage {
        [SerializeReference]
        private IDamageSource source;

        [SerializeField]
        private uint original;

        public IDamageSource Source => source;

        public uint Original => original;

        public Damage(
            IDamageSource source,
            uint amount
        ) {
            this.source = source;
            original = amount;
        }
    }

    public interface IDamageSource {
    }

    public interface IDefendable : IDamageSource {
        float Defend(Combatant defendant);
        float Counter(Combatant defendant);
    }
}