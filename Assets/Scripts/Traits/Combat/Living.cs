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
        private bool alive;
        public UnityEvent onDeath;
        public bool initiallyAlive = true;
        private void Awake() {
            alive = initiallyAlive;
        }

        public bool Alive {
            get => alive;
            set {
                if (alive == value) {
                    return;
                }
                alive = value;
                if (!alive) {
                    onDeath.Invoke();
                }
            }
        }

        public bool Dead {
            get => !Alive;
            set => Alive = !value;
        }

        public void Kill() {
            Dead = true;
        }
    }
}