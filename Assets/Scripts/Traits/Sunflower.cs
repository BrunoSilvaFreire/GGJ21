using System;
using System.Collections.Generic;
using GGJ.Game;
using GGJ.Input;
using GGJ.Traits.Animation;
using GGJ.Traits.Combat;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Misc;
using UnityEngine;

namespace GGJ.Traits {
    public class Sunflower : Trait {
        public float range = 7;
        public float projectileSpeed = 5;
        public Clock cooldown;
        public Projectile prefab;
        private Combatant self;
        private EntityInput input;
        private bool shooting;
        private AnimatorBinder binder;
        private Vector2Int shootDir;
        public Transform up, side;
        private List<Projectile> projectiles;
        private void Awake() {
            projectiles = new List<Projectile>();
        }

        private void OnDrawGizmosSelected() {
            Gizmos2.DrawWireCircle2D(transform.position, range, Color.red);
        }
        public override void Configure(TraitDependencies dependencies) {
            dependencies.DependsOn(out self, out input, out binder);

        }
        private void Update() {
            var target = Player.Instance.Pawn;
            if (target == null) {
                return;
            }
            if (shooting) {
                return;
            }
            Vector2Int dir = default;


            if (TryGetDirectionTo(target, ref dir)) {
                if (cooldown.Tick()) {
                    shootDir = dir;
                    shooting = true;
                    binder.Animator.SetTrigger(dir.y == 1 ? "ShootUp" : "ShootHorizontal");
                }
            }
        }

        public void Shoot() {
            shooting = false;
            Transform attachment;
            if (shootDir.y == 1) {
                attachment = up;
            } else {
                attachment = side;
                var pos = side.localPosition;
                switch (shootDir.x) {
                    case -1:
                        if (pos.x > 0) {
                            pos.x *= -1;
                        }
                        break;
                    case 1:
                        if (pos.x < 0) {
                            pos.x *= -1;
                        }
                        break;
                }
                side.transform.localPosition = pos;
            }
            input.horizontal = shootDir.x;
            input.vertical = shootDir.y;
            var projectile = prefab.Clone(attachment.position);
            projectile.maxDistance = range;
            projectile.Shoot(self, projectileSpeed);
            projectiles.Add(projectile);
        }
        private bool TryGetDirectionTo(Entity target, ref Vector2Int result) {
            var direction = target.transform.position - transform.position;
            if (direction.magnitude > range) {
                return false;
            }
            var dir = Vector2Int.RoundToInt(direction.normalized);
            if (dir.x != 0 && dir.y != 0) {
                return false;
            }
            result = dir;
            return true;
        }
        // public void Setup(Map obj) {
        //     obj.PlayerInside.Bind(delegate(bool inside) {
        //         enabled = inside;
        //         if (!inside) {
        //             foreach (var projectile in projectiles) {
        //                 if (projectile != null) {
        //                     Destroy(projectile);
        //                 }
        //             }
        //             projectiles.Clear();
        //         }
        //     });
        // }
    }
}