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
        public bool Dead {
            get => !alive;
            set => alive = !value;
        }

        public void Kill() {
            Dead = false;
        }
    }
}