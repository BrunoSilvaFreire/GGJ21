using System;
using Common;
using GGJ.Traits.Combat;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Misc;
using Movement;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Master.Movements {
    public class AttackAttachment : MotorState {
        public Bounds2D attackBounds;
        public Animator animator;
        public UnityEvent onAttack;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private void OnDrawGizmosSelected() {
            Gizmos2.DrawBounds2D(Bounds(GetComponentInParent<Entity>().GetComponentInChildren<Combatant>()));
        }

        public override void Begin(Motor motor) { }

        public override void Tick(Motor motor) {
            if (motor.entityInput.attack.Continuous.JustActivated) {
                if (motor.Owner.Access(out Combatant combatant)) {
                    Perform(combatant);
                }
            }
        }

        public override void End(Motor motor) { }
        public Bounds2D Bounds(Combatant combatant) {
            var b = attackBounds;
            if (combatant && combatant.Owner && combatant.Owner.Access(out Motor m)) {
                b.center.x *= m.lastDirection;
            }

            b.center += (Vector2)combatant.transform.position;
            return b;
        }

        public void Perform(Combatant combatant) {
            var b = Bounds(combatant);
            var mask = GameConfiguration.Instance.attackableMask;
            Debugging.DrawBounds2D(b, Color.red);
            var found = Physics2D.OverlapBoxAll(b.center, b.size, 0, mask);
            animator.SetTrigger(Attack);
            onAttack.Invoke();
            foreach (var other in found) {
                var entity = other.GetComponentInParent<Entity>();
                if (entity == null) {
                    continue;
                }

                if (!entity.Access(out Living living)) {
                    continue;
                }

                if (!combatant.CanAttack(living)) {
                    continue;
                }
                living.Kill();
            }
        }
    }
}