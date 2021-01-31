using Input;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Traits.Combat {
    public class Projectile : Trait {
        public Vector2 velocity;
        public float velocityMultiplier = 1;
        public Effect deflectEffect, counterEffect, sparkleEffect;

        [Required]
        public new Rigidbody2D rigidbody;

        public float maxDistance = 30;
        private float travelled;
        public bool bypassCooldown;

        [Required]
        public Combatant shooter;

        public uint damage = 3;
        public float defenseSpeedMultiplier = 3;
        public bool destroyOnCollision = true;
        public UnityEvent onHit;
        public TrailRenderer trail;
        public ParticleSystem[] systemsPrefabs;
        private ParticleSystem[] systems;
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.isTrigger) {
                return;
            }

            if (Owner.Access(out Living own)) {
                if (own.Dead) {
                    return;
                }
            }

            var entity = other.GetComponentInParent<Entity>();
            if (entity == null) {
                var pos = other.ClosestPoint(transform.position);
                sparkleEffect.PlayIfPresent(
                    this,
                    false,
                    new PositionFeature(pos)
                );
                EndLifetime();
                return;
            }

            if (!entity.Access(out Living l)) {
                return;
            }

            if (!shooter.CanAttack(l)) {
                return;
            }
            l.Kill();
            EndLifetime();
        }

        private void EndLifetime() {
            travelled = 0;
            foreach (var system in systems) {
                system.Stop(true);
            }
            if (trail != null) {
                trail.Clear();
            }

            onHit.Invoke();
            if (destroyOnCollision) {
                Owner.Delete();
            }
        }

        private void Start() {
            rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigidbody.isKinematic = true;
            rigidbody.useFullKinematicContacts = true;
            systems = new ParticleSystem[systemsPrefabs.Length];
            for (var i = 0; i < systemsPrefabs.Length; i++) {
                var prefab = systemsPrefabs[i];
                ParticleSystem system;
                systems[i] = system = prefab.Clone(transform);
                system.transform.localPosition = Vector3.zero;
            }
        }

        private void FixedUpdate() {
            rigidbody.velocity = velocity * velocityMultiplier;
            travelled += (velocity * (Time.fixedDeltaTime * velocityMultiplier)).magnitude;
            if (travelled > maxDistance) {
                EndLifetime();
            }
        }

        public void Shoot(Combatant combatant, float speed) {
            if (!combatant.Owner.Access(out EntityInput input)) {
                Debug.LogError("Cannot shot profile because " + combatant + " has no EntityInput component");
                return;
            }


            var direction = new Vector2(input.horizontal, input.vertical);
            direction.Normalize();
            direction *= speed;
            velocity = direction;
            shooter = combatant;
        }
    }
}