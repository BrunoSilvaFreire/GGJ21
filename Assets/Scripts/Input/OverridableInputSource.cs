using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Input {
    public class OverridableInputSource : InputSource {
        public float horizontal, vertical;
        public bool jump, interact, cancel;

        [Flags]
        public enum InputOverrideFlags {
            Horizontal = 1 << 0,
            Vertical = 1 << 1,
            Jump = 1 << 2,
            Interact = 1 << 3,
            Cancel = 1 << 4
        }

        //0010 1111
        public InputOverrideFlags overriden;

        [Required]
        public InputSource delegateSource;

        public Vector2 aim;

        public void SetOverridenAsInt(int value) {
            overriden = (InputOverrideFlags)value;
        }

        private bool IsOverriden(InputOverrideFlags which) {
            return (overriden & which) == which;
        }

        private T Evaluate<T>(InputOverrideFlags which, T owned, Func<T> fallback) {
            return IsOverriden(which) ? owned : fallback();
        }

        public override float GetHorizontal() {
            return Evaluate(InputOverrideFlags.Horizontal, horizontal, delegateSource.GetHorizontal);
        }


        public override float GetVertical() {
            return Evaluate(InputOverrideFlags.Vertical, vertical, delegateSource.GetVertical);
        }
        public override bool GetCancel() {
            return Evaluate(InputOverrideFlags.Cancel, cancel, delegateSource.GetCancel);
        }

        public override bool GetJump() {
            return Evaluate(InputOverrideFlags.Jump, jump, delegateSource.GetJump);
        }


        public override bool GetInteract() {
            return Evaluate(InputOverrideFlags.Interact, interact, delegateSource.GetInteract);
        }

        public void Reset() {
            interact = false;
            jump = false;
            horizontal = 0;
            vertical = 0;
        }
    }
}