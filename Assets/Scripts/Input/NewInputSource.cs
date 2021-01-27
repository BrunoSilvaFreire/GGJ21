using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Input {
    public class NewInputSource : InputSource {
        public PlayerInput input;
        private InputAction horizontal, vertical, jump, interact;
        public Transform mouseAnchor;
        [Required]
        public new Camera camera;
        private void Awake() {
            horizontal = input.actions["Horizontal"];
            vertical = input.actions["Vertical"];
            jump = input.actions["Jump"];
            interact = input.actions["Interact"];
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