using BehaviorDesigner.Runtime;
using GGJ.Common;
using GGJ.Game;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime.Misc;
using UnityEngine;

namespace GGJ.Master.AI {
    public class ThreatSensor : Trait {
        private BehaviorTree tree;
        public float sensor = 5;
        public Clock clock = 0.25F;
        public override void Configure(TraitDependencies dependencies) {
            tree = dependencies.RequiresComponent<BehaviorTree>("");
        }
        private void Update() {
            if (clock.Tick()) {
                if (tree.GetVariable("target") is SharedEntity exiting && exiting.Value != null) {
                    return;
                }

                var found = Physics2D.OverlapCircleAll(
                    transform.position,
                    sensor,
                    GameConfiguration.Instance.attackableMask
                );

                foreach (var col in found) {
                    var entity = col.GetComponent<Entity>();
                    if (entity == null) {
                        continue;
                    }
                    if (entity != Player.Instance.Pawn) {
                        continue;
                    }
                    tree.SetVariable(
                        "target",
                        new SharedEntity {
                            Value = entity
                        }
                    );
                    BehaviorManager.instance.RestartBehavior(tree);
                }
            }
        }
    }
}