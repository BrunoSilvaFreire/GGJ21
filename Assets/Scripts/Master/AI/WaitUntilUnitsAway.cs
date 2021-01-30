using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;
namespace GGJ.Master.AI {
    [Serializable]
    public class WaitUntilUnitsAway : Action {
        public SharedGameObject obj;
        public float distance;
        private Vector2 reference;
        public override void OnStart() {
            reference = obj.Value.transform.position;
        }
        public override TaskStatus OnUpdate() {
            if (Vector2.Distance(obj.Value.transform.position, reference) < distance) {
                return TaskStatus.Running;
            }
            return TaskStatus.Success;
        }
    }
}