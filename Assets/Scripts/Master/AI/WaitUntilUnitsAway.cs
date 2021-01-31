using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Movement;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;
namespace GGJ.Master.AI {
    [Serializable]
    public class WaitUntilUnitsAway : Action {
        public SharedEntity entity;
        public float distance;
        private Vector2 reference;
        private Motor motor;
        public override void OnStart() {
            reference = entity.Value.transform.position;
            motor = entity.Value.GetTrait<Motor>();
        }
        public override TaskStatus OnUpdate() {
            if (motor != null) {
                var dir = Math.Sign(motor.entityInput.horizontal);
                var col = motor.supportState.Horizontal;
                if (dir == col) {
                    return TaskStatus.Success;
                }
            }

            if (Vector2.Distance(entity.Value.transform.position, reference) < distance) {
                return TaskStatus.Running;
            }
            return TaskStatus.Success;
        }
    }
}