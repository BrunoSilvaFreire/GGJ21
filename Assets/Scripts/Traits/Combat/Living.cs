using System;
using Lunari.Tsuki.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Traits.Combat {
    [Serializable]
    public sealed class HealthChangeEvent : UnityEvent<uint> { }

    [Serializable]
    public sealed class DeathEvent : UnityEvent { }


    [TraitLocation(TraitLocations.Combat)]
    public class Living : Trait {
        public EntityEvent onDeath;
        public bool initiallyAlive = true;
        public bool canBeHurtByHazard;
        public bool invincible;
        private void Awake() {
            Alive = initiallyAlive;
        }

        public bool Alive {
            get;
            set;
        }

        public bool Dead {
            get => !Alive;
            private set => Alive = !value;
        }

        public void Kill(Entity killer = null) {
            if (Dead || invincible) {
                return;
            }
            Dead = true;
            onDeath.Invoke(killer);
        }
    }
}