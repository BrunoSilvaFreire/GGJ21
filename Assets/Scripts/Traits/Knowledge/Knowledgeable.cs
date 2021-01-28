using System;
using System.Collections.Generic;
using Lunari.Tsuki.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Traits.Knowledge {
    public class Knowledgeable : Trait {
        [SerializeField, HideInInspector]
        private Knowledge currentKnowledge = Knowledge.Jump | Knowledge.MoveLeft | Knowledge.MoveRight;

        private Dictionary<Knowledge, UnityEvent<bool>> binds;
        public UnityEvent onKnowledgeChanged = new UnityEvent();
        public uint maxNumberOfKnowledge = 3;
        private void Start() {
            foreach (var bind in binds) {
                ConfigureBind(currentKnowledge, bind, 0);
            }
        }

        public void Bind(Knowledge flags, UnityAction<bool> onChanged) {
            binds ??= new Dictionary<Knowledge, UnityEvent<bool>>();
            if (!binds.TryGetValue(flags, out var bind)) {
                bind = new UnityEvent<bool>();
                binds[flags] = bind;
            }
            onChanged(Matches(flags));

            bind.AddListener(onChanged);
        }

        [Flags]
        public enum Knowledge : ushort {
            MoveLeft = 1 << 0,
            MoveRight = 1 << 1,
            Jump = 1 << 2,
            LookUp = 1 << 3,
            LookDown = 1 << 4,
            // Crouch = 1 << 5,
            Interact = 1 << 6,
            Attack = 1 << 7,
            WallJump = 1 << 8,
            DoubleJump = 1 << 9
        }
        public bool Matches(Knowledge required) {
            return Matches(currentKnowledge, required);
        }
        public static bool Matches(Knowledge current, Knowledge required) {
            return (current & required) == required;
        }

        [ShowInInspector]
        public Knowledge CurrentKnowledge {
            get => currentKnowledge;
            set {
                if (currentKnowledge == value) {
                    return;
                }
                var old = currentKnowledge;
                currentKnowledge = value;
                onKnowledgeChanged.Invoke();
                if (binds != null) {
                    foreach (var keyValuePair in binds) {
                        ConfigureBind(value, keyValuePair, old);
                    }
                }
            }
        }

        private static void ConfigureBind(Knowledge value, KeyValuePair<Knowledge, UnityEvent<bool>> keyValuePair, Knowledge old) {
            var knowledge = keyValuePair.Key;
            var previous = Matches(old, knowledge);
            var now = Matches(value, knowledge);
            if (previous != now) {
                keyValuePair.Value.Invoke(now);
            }
        }
    }
}