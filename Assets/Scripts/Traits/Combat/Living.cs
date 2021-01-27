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

    public delegate void DamageModifier(DamageEvent damage);


    [TraitLocation(TraitLocations.Combat)]
    public class Living : Trait {
        public const string StatsGroup = "Stats";
        public const string ExtrasGroup = "Extras";
        public const string EventsGroup = "Events";
        public const string FunctionsGroup = "Functions";

        [SerializeField, HideInInspector]
        private uint health, maxHealth;

        public event DamageModifier onPreDamage, onPostDamage;

        [FoldoutGroup(EventsGroup, order: 5)]
        public HealthChangeEvent onHealthChanged;

        [FoldoutGroup(EventsGroup)]
        public DeathEvent onDeath;

        [BoxGroup(ExtrasGroup)]
        public float damageCooldown;

        [ShowInInspector, BoxGroup(StatsGroup)]

        public uint Health {
            get => health;
            private set {
                var newValue = Math.Min(value, maxHealth);
                health = newValue;
            }
        }

        [FoldoutGroup(FunctionsGroup)]
        public DamageEvent Damage(Damage damage, bool bypassCooldown = false) {
            var e = new DamageEvent(damage, this);
            onPreDamage?.Invoke(e);

            var value = e.Value;
            if (value == 0) {
                return e;
            }

            if (!bypassCooldown) {
                if (CurrentDamageCooldown > 0) {
                    return e;
                }
            }

            if (value >= health) {
                onHealthChanged.Invoke(0);
                Kill();
            } else {
                onHealthChanged.Invoke(health - value);
                health -= value;
            }

            onPostDamage?.Invoke(e);
            CurrentDamageCooldown = damageCooldown;
            return e;
        }

        [FoldoutGroup(FunctionsGroup)]
        public void Kill() {
            onDeath.Invoke();
            health = 0;
            Owner.Aware = false;
        }

        [ShowInInspector, BoxGroup(StatsGroup, order: 0)]
        public uint MaxHealth {
            get => maxHealth;
            set {
                if (value < health) {
                    health = value;
                }

                maxHealth = value;
            }
        }

        public float CurrentDamageCooldown { get; private set; }

        public bool Dead => health == 0;

        private void Update() {
            if (CurrentDamageCooldown > 0) {
                CurrentDamageCooldown -= Time.deltaTime;
            }
        }

        public void Heal(uint healAmount) {
            var newHealth = Math.Min(health + healAmount, maxHealth);
            onHealthChanged.Invoke(newHealth);
            health = newHealth;
        }
    }
}