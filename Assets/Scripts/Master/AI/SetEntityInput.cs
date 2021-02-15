using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Action = BehaviorDesigner.Runtime.Tasks.Action;
namespace GGJ.Master.AI {
    [Serializable]
    public class SetEntityInput : Action {

        public SharedEntityInput input;
        public List<Entry> entries;

        [Serializable]
        public struct Entry {
            public float floatValue;
            public bool boolValue;
            public Input input;
            public void Deconstruct(out float floatValue, out bool boolValue, out Input input) {
                floatValue = this.floatValue;
                boolValue = this.boolValue;
                input = this.input;
            }
        }
        public enum Input {
            Horizontal,
            Vertical,
            Jump,
            Interact
        }
        public override TaskStatus OnUpdate() {
            var value = input.Value;
            if (value == null) {
                return TaskStatus.Failure;
            }
            foreach (var (floatValue, boolValue, bindPoint) in entries) {
                switch (bindPoint) {
                    case Input.Horizontal:
                        value.horizontal = floatValue;
                        break;
                    case Input.Vertical:
                        value.vertical = floatValue;
                        break;
                    case Input.Jump:
                        value.jump.Current = boolValue;

                        break;
                    case Input.Interact:
                        value.interacting.Current = boolValue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return TaskStatus.Success;
        }
    }
}