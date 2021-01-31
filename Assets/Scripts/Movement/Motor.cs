using System;
using System.Collections.Generic;
using Common;
using Input;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime.Exceptions;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Movement {
    [Flags]
    public enum DirectionFlags : byte {
        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        Horizontal = Left | Right,
        Vertical = Up | Down,
        All = Horizontal | Vertical
    }

    public abstract class MotorState : MonoBehaviour {
        public MotorEvent onBegin, onEnd;

        public virtual bool CanTransitionInto(Motor motor) {
            return true;
        }

        public virtual void Begin(Motor motor) { }

        public virtual void Tick(Motor motor) { }

        public virtual void End(Motor motor) { }
        public void TryTick(Motor motor) {
            if (enabled) {
                Tick(motor);
            }
        }
    }


    [Serializable]
    public struct SupportState : IComparable<SupportState> {
        // [EnumToggleButtons]
        public DirectionFlags flags;

        private bool GetBit(DirectionFlags f) {
            return (flags & f) == f;
        }

        private void SetBit(DirectionFlags directionFlags, bool value) {
            if (value) {
                flags |= directionFlags;
            } else {
                flags &= ~directionFlags;
            }
        }

        public bool up {
            get => GetBit(DirectionFlags.Up);
            set => SetBit(DirectionFlags.Up, value);
        }


        public bool down {
            get => GetBit(DirectionFlags.Down);
            set => SetBit(DirectionFlags.Down, value);
        }

        public bool left {
            get => GetBit(DirectionFlags.Left);
            set => SetBit(DirectionFlags.Left, value);
        }

        public bool right {
            get => GetBit(DirectionFlags.Right);
            set => SetBit(DirectionFlags.Right, value);
        }

        public int CompareTo(SupportState other) {
            var upComparison = up.CompareTo(other.up);
            if (upComparison != 0) {
                return upComparison;
            }

            var downComparison = down.CompareTo(other.down);
            if (downComparison != 0) {
                return downComparison;
            }

            var leftComparison = left.CompareTo(other.left);
            if (leftComparison != 0) {
                return leftComparison;
            }

            return right.CompareTo(other.right);
        }

        public bool Equals(SupportState other) {
            return flags == other.flags;
        }

        public override bool Equals(object obj) {
            return obj is SupportState other && Equals(other);
        }

        public override int GetHashCode() {
            return (int)flags;
        }

        public static bool operator ==(SupportState a, SupportState b) {
            return a.CompareTo(b) == 0;
        }

        public static bool operator !=(SupportState a, SupportState b) {
            return !(a == b);
        }

        public int Horizontal {
            get {
                if (left == right) {
                    return 0;
                }

                if (left) {
                    return -1;
                }

                if (right) {
                    return 1;
                }

                return 0;
            }
        }

        public bool Any => !None;
        public bool None => flags == 0;

        public bool HasCollisionOnDirection(Vector2 velocity) {
            return GetBit(GetXDirectionFlags(velocity.x) | GetYDirectionFlags(velocity.y));
        }

        private DirectionFlags GetXDirectionFlags(float x) {
            switch (Math.Sign(x)) {
                case 1:
                    return DirectionFlags.Right;
                case -1:
                    return DirectionFlags.Left;
                default:
                    return 0;
            }
        }

        private DirectionFlags GetYDirectionFlags(float y) {
            switch (Math.Sign(y)) {
                case 1:
                    return DirectionFlags.Up;
                case -1:
                    return DirectionFlags.Down;
                default:
                    return 0;
            }
        }
    }
    public enum HorizontalDirection : sbyte {
        Left = -1,
        None = 0,
        Right = 1
    }

    [TraitLocation("Movement")]
    public partial class Motor : Trait {
        [NonSerialized]
        public EntityInput entityInput;

        public int lastDirection { get; set; }
        public FloatProperty maxSpeed;

        [NonSerialized]
        public new Rigidbody2D rigidbody;

        [NonSerialized]
        public new CapsuleCollider2D collider;
        public HorizontalDirection preferredDirection;
        public HorizontalDirection GetDirection() {
            if (preferredDirection != HorizontalDirection.None) {
                return preferredDirection;
            }
            var dir = rigidbody.velocity.x;
            if (dir > 0) {
                return HorizontalDirection.Left;
            }

            if (dir < 0) {
                return HorizontalDirection.Right;
            }
            return HorizontalDirection.None;
        }
        public override void Configure(TraitDependencies dependencies) {
            dependencies.DependsOn(out entityInput);
            rigidbody = dependencies.RequiresComponent<Rigidbody2D>("");
            collider = dependencies.RequiresComponent<CapsuleCollider2D>("");
        }

        public SupportState supportState;

        [SerializeField, Required, InlineEditor()]
        private MotorState activeState;

        public MotorState[] permanentStates;

        [SerializeField]
        private float control = 1;
        [SerializeField]
        private float leftControl = 1, rightControl = 1;

        public ref float GetDirectionControlReference(int direction) {
            switch (direction) {
                case -1:
                    return ref leftControl;
                case 1:
                    return ref rightControl;
            }
            throw new WTFException("Last direction is 0");
        }
        public ref float GetDirectionControlReference() {
            return ref GetDirectionControlReference(lastDirection);
        }


        public float GetDirectionControl(int direction) {
            switch (direction) {
                case -1:
                    return leftControl * control;
                case 1:
                    return rightControl * control;
            }
            return control;
        }
        private MotorState nextState;

        public MotorState ActiveState {
            get => activeState;
            set => nextState = value;
        }

        public float Control {
            get => control;
            set => control = value;
        }

        public float LeftControl {
            get => leftControl;
            set => leftControl = value;
        }

        public float RightControl {
            get => rightControl;
            set => rightControl = value;
        }

        private void Start() {
            lastDirection = 1;
            if (ActiveState != null) {
                DoBeginOn(ActiveState);
            }
            foreach (var permanentState in permanentStates) {
                DoBeginOn(permanentState);
            }

        }

        private void DoBeginOn(MotorState state) {
            state.Begin(this);
            state.onBegin.Invoke(this);
        }

        private void DoEndOn(MotorState state) {
            state.End(this);
            state.onEnd.Invoke(this);
        }

        private void FixedUpdate() {
            ConsumeAndUpdateSupportState();
            activeState.TryTick(this);
            foreach (var permanentState in permanentStates) {
                permanentState.TryTick(this);
            }

            if (nextState != null && activeState != nextState) {
                if (!nextState.CanTransitionInto(this)) {
                    nextState = null;
                    return;
                }

                if (activeState != null) {
                    DoEndOn(activeState);
                }

                activeState = nextState;
                if (nextState != null) {
                    DoBeginOn(activeState);
                }
            }

            var vel = rigidbody.velocity.x;
            var dir = Math.Sign(vel);
            if (dir != 0) {
                lastDirection = dir;
            }
            // rigidbody.AddForce(externalForces, ForceMode2D.Impulse);
        }

    }
}