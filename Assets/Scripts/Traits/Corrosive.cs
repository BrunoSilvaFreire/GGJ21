using System;
using System.Collections.Generic;
using Common;
using GGJ.Traits.Combat;
using Lunari.Tsuki.Entities;
using Shiroi.FX.Effects;
using Sirenix.OdinInspector;
using UnityEngine;
namespace GGJ.Traits {
    [TraitLocation(TraitLocations.Combat)]
    public class Corrosive : Trait {
        public CollisionMode mode = CollisionMode.Collision;

        [ShowIf(nameof(IsManualQuery))]
        public Collider2D query;

        [ShowIf(nameof(IsManualQuery))]
        public LayerMask queryMask;
        private Combatant self;

        public Effect onDamageEffect;

        [NonSerialized]
        private readonly List<Collider2D> manualQueryWhiteList = new List<Collider2D>();

        private bool IsManualQuery() {
            return mode.HasFlag(CollisionMode.ManualQuery);
        }

        public override void Configure(TraitDependencies dependencies) {
            self = dependencies.GetTrait<Combatant>();
        }

        private void FixedUpdate() {
            if (!IsManualQuery()) {
                return;
            }

            var filter = new ContactFilter2D {
                layerMask = queryMask,
                useLayerMask = true,
                useTriggers = false
            };
            var elements = new Collider2D[4];
            var overlaps = query.OverlapCollider(filter, elements);
            for (var i = 0; i < overlaps; i++) {
                var found = elements[i];
                if (manualQueryWhiteList.Contains(found)) {
                    continue;
                }

                TryDamage(found);
            }

            manualQueryWhiteList.Clear();
            for (var i = 0; i < overlaps; i++) {
                manualQueryWhiteList.Add(elements[i]);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (mode.HasFlag(CollisionMode.Trigger)) {
                TryDamage(other);
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (mode.HasFlag(CollisionMode.Collision)) {
                TryDamage(other.collider);
            }
        }

        private void TryDamage(Collider2D other) {
            if (Owner.Access(out Living own)) {
                if (own.Dead) {
                    return;
                }
            }

            var e = other.GetComponentInParent<Entity>();
            if (e == null) {
                return;
            }

            if (!e.Access(out Living l)) {
                return;
            }

            if (self != null) {
                if (e.Access(out Combatant enemy)) {
                    if (!self.CanAttack(enemy)) {
                        return;
                    }
                }
            }

            l.Kill(Owner);
            onDamageEffect.PlayIfPresent(e);
        }
    }
}