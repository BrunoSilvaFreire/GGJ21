using System;
using BehaviorDesigner.Runtime.Tasks;
using Action = BehaviorDesigner.Runtime.Tasks.Action;
namespace GGJ.Master.AI {
    [Serializable]
    public class GoToTarget : Action {
        public SharedEntityInput input;
        public SharedEntity target;
        public override TaskStatus OnUpdate() {
            var tgt = target.Value;
            if (tgt == null) {
                return TaskStatus.Failure;
            }
            var ipt = input.Value;
            if (ipt == null) {
                return TaskStatus.Failure;
            }
            ipt.horizontal = Math.Sign(tgt.transform.position.x - transform.position.x);
            return TaskStatus.Success;
        }
    }
}