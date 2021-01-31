using System;
using Common;
using Lunari.Tsuki.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Input {
    public class DiscreteInputState {
        [ShowInInspector]
        private bool set;

        public void Set() {
            set = true;
        }

        public bool Consume() {
            if (!set) {
                return false;
            }

            set = false;
            return true;
        }

        public bool Peek() {
            return set;
        }

        public bool MaybeConsume() {
            return Peek() && Consume();
        }
    }

    public class ContinuousInputState : BooleanHistoric { }

    public abstract class InputSource : MonoBehaviour {
        public abstract float GetHorizontal();
        public abstract bool GetJump();
        public abstract bool GetInteract();
        public abstract float GetVertical();
        public abstract bool GetCancel();
        public abstract bool GetReset();
        public abstract bool GetAttack();
    }

    [Serializable]
    public class MixedInputState {
        [ShowInInspector] public ContinuousInputState Continuous { get; }

        [ShowInInspector] public DiscreteInputState Discrete { get; }
        public bool overriden;
        public bool overwriteValue;

        [ShowInInspector]
        public bool Current {
            get {
                if (overriden) {
                    return overwriteValue;
                }

                return Continuous.Current;
            }
            set {
                Continuous.Current = value;

                if (Continuous.JustActivated) {
                    Discrete.Set();
                }
            }
        }

        public MixedInputState() {
            Discrete = new DiscreteInputState();
            Continuous = new ContinuousInputState();
        }
    }

    public class EntityInput : Trait {
        public float horizontal;
        public float vertical;
        public MixedInputState jump;
        public MixedInputState interacting;
        public bool locked;
        public InputSource source;
        public MixedInputState attack;

        public Vector2 Direction {
            get => new Vector2(horizontal, vertical);
            set {
                horizontal = value.x;
                vertical = value.y;
            }
        }

        private void Awake() {
            jump = new MixedInputState();
            interacting = new MixedInputState();
        }

        private void Update() {
            var src = source;
            if (src == null) {
                return;
            }

            Transfer(src);
        }


        private void Transfer(InputSource src) {
            horizontal = src.GetHorizontal();
            vertical = src.GetVertical();
            jump.Current = src.GetJump();
            interacting.Current = src.GetInteract();
            attack.Current = src.GetAttack();
        }

        public void Reset() {
            horizontal = 0;
            vertical = 0;
            jump.Current = false;
        }
    }
}