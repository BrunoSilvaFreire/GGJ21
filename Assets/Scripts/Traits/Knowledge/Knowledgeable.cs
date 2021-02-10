using System;
using System.Collections.Generic;
using Common;
using GGJ.Master;
using Lunari.Tsuki.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Traits.Knowledge {
    [Serializable]
    public class BindableKnowledge : Bindable<Knowledgeable.Knowledge> { }

    public class Knowledgeable : Trait, IPersistant {
        [SerializeField, HideInInspector]
        private BindableKnowledge currentKnowledge;

        private Knowledge savedKnowledge;

        private Dictionary<Knowledge, UnityEvent<bool>> binds;
        [SerializeField, HideInInspector]
        private UIntBindable maxNumberOfKnowledge;
        private Knowledge old;

        private void Awake() {
            currentKnowledge.onChanged.AddListener(delegate {
                var value = currentKnowledge.Value;
                if (binds != null) {
                    foreach (var keyValuePair in binds) {
                        ConfigureBind(value, keyValuePair, old);
                    }
                }
                old = currentKnowledge.Value;
            });
        }
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
            None = 0,
            MoveLeft = 1 << 0,
            MoveRight = 1 << 1,
            Jump = 1 << 2,
            LookUp = 1 << 3,
            LookDown = 1 << 4,
            Glide = 1 << 5,
            Dodge = 1 << 6,
            Attack = 1 << 7,
            WallJump = 1 << 8,
            SuperJump = 1 << 9,
            Roll = 1 << 10,
            MoveHorizontally = MoveLeft | MoveRight,
            Platform = MoveHorizontally | Jump,
            All = 0b0000011111111111
        }
        public bool Matches(Knowledge required) {
            return Matches(currentKnowledge, required);
        }
        public static bool Matches(Knowledge current, Knowledge required) {
            return (current & required) == required;
        }

        [ShowInInspector]
        public BindableKnowledge CurrentKnowledge {
            get => currentKnowledge;
        }

        [ShowInInspector]
        public Knowledge KnowledgeValue {
            get => currentKnowledge.Value;
            set => currentKnowledge.Value = value;
        }

        private PersistanceManager m_manager;

        [ShowInInspector]
        public UIntBindable MaxNumberOfKnowledge {
            get => maxNumberOfKnowledge;
            set => maxNumberOfKnowledge.Value = value;
        }

        private static void ConfigureBind(Knowledge value, KeyValuePair<Knowledge, UnityEvent<bool>> keyValuePair, Knowledge old) {
            var knowledge = keyValuePair.Key;
            var previous = Matches(old, knowledge);
            var now = Matches(value, knowledge);
            if (previous != now) {
                keyValuePair.Value.Invoke(now);
            }
        }

        public void ConfigurePersistance(PersistanceManager manager) {
            m_manager = manager;
            m_manager.onSave.AddListener(OnSave);
            m_manager.onLoad.AddListener(OnLoad);
            OnSave(); //saves current state
        }

        private void OnLoad() {
            currentKnowledge.Value = savedKnowledge;
        }

        private void OnSave() {
            savedKnowledge = currentKnowledge;
        }
    }
}