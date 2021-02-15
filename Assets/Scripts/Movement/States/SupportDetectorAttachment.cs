using System;
using System.Collections.Generic;
using System.Linq;
using GGJ.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Movement.States {
    [Serializable]
    public class SupportListener {
        public SupportRequirementNode requirement;
        public UnityEvent onActive;

        public bool Allowed(SupportState state) {
            return requirement.IsMet(state);
        }
    }

    [Serializable]
    public struct SupportRequirementNode {
        [HideIf(nameof(IsSelf))]
        public List<SupportRequirementNode> children;

        [ShowIf(nameof(IsSelf))]
        public SupportRequirement requirement;

        public BitMode mode;

        private bool IsSelf() {
            return mode == BitMode.Self;
        }

        public enum BitMode {
            Self,
            All,
            Any,
            None
        }

        public bool IsMet(SupportState supportState) {
            switch (mode) {
                case BitMode.All:
                    return children.All(node => node.IsMet(supportState));
                case BitMode.Any:
                    return children.Any(node => node.IsMet(supportState));
                case BitMode.None:
                    return !children.Any(node => node.IsMet(supportState));
                case BitMode.Self:
                    return requirement.IsMet(supportState);
                default:
                    return false;
            }
        }
    }

    [Serializable]
    public struct SupportRequirement {
        public SupportState state;
        public bool value;


        public bool IsMet(SupportState supportState) {
            return ((state.flags & supportState.flags) == state.flags) == value;
        }
    }

    public class SupportDetectorAttachment : MotorState {
        public List<SupportListener> listeners;
        [ShowInInspector, SerializeReference]
        public Filter filters;

        public override void Tick(Motor motor) {
            EvaluateListeners(motor);
        }

        private void EvaluateListeners(Motor m) {
            if (filters != null && !filters.Allowed(m.collider)) {
                return;
            }

            foreach (var supportListener in listeners) {
                if (!supportListener.Allowed(m.supportState)) {
                    continue;
                }

                supportListener.onActive.Invoke();
            }
        }
    }
}