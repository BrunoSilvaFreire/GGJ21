using UnityEngine;
namespace Input {
    public class OverridableInputSourceProxy : MonoBehaviour {
        public OverridableInputSource target;

        public void SetHorizontal(float value) {
            if (target == null) {
                return;
            }

            target.horizontal = value;
        }

        public void SetVertical(float value) {
            if (target == null) {
                return;
            }

            target.vertical = value;
        }

        public void SetJump(bool value) {
            if (target == null) {
                return;
            }

            target.jump = value;
        }

        public void SetOverriden(OverridableInputSource.InputOverrideFlags value) {
            if (target == null) {
                return;
            }

            target.overriden = value;
        }

        public void SetDelegateSource(InputSource value) {
            if (target == null) {
                return;
            }

            target.delegateSource = value;
        }

        public void SetAim(Vector2 value) {
            if (target == null) {
                return;
            }

            target.aim = value;
        }

        public void SetUseGUILayout(bool value) {
            if (target == null) {
                return;
            }

            target.useGUILayout = value;
        }

        public void SetRunInEditMode(bool value) {
            if (target == null) {
                return;
            }

            target.runInEditMode = value;
        }

        public void SetEnabled(bool value) {
            if (target == null) {
                return;
            }

            target.enabled = value;
        }

        public void SetTag(string value) {
            if (target == null) {
                return;
            }

            target.tag = value;
        }

        public void SetName(string value) {
            if (target == null) {
                return;
            }

            target.name = value;
        }

        public void SetHideFlags(HideFlags value) {
            if (target == null) {
                return;
            }

            target.hideFlags = value;
        }
    }
}