using UnityEngine.InputSystem;
namespace GGJ.Input {
    public class NewInputSource : InputSource {
        public PlayerInput input;
        private InputAction horizontal, vertical, jump, interact, cancel, reset, attack;
        private void Awake() {
            horizontal = input.actions["Horizontal"];
            vertical = input.actions["Vertical"];
            jump = input.actions["Jump"];
            interact = input.actions["Interact"];
            cancel = input.actions["Cancel"];
            reset = input.actions["Reset"];
            attack = input.actions["Attack"];
        }
        public override bool GetCancel() {
            return GetButton(cancel);
        }

        public override bool GetReset() {
            return GetButton(reset);
        }
        public override bool GetAttack() {
            return GetButton(attack);
        }

        public override float GetHorizontal() {
            return horizontal.ReadValue<float>();
        }


        public override float GetVertical() {
            return vertical.ReadValue<float>();
        }

        private static bool GetButton(InputAction action) {
            return action.ReadValue<float>() > 0.1;
        }

        public override bool GetJump() {
            return GetButton(jump);
        }


        public override bool GetInteract() {
            return GetButton(interact);
        }
    }
}