using Common;
using UnityEngine;
namespace Movement {
    public class MotorProxy : MonoBehaviour {
        public Motor target;

        public void SetSpeedMultiplier(float multiplier) {
            if (target == null) {
                Debug.LogWarning("Null Tardet");
                return;
            }

            target.maxSpeed.masterMultiplier = multiplier;
        }



        public void SetSupportState(SupportState value) {
            if (target == null) {
                return;
            }

            target.supportState = value;
        }

        public void SetActiveState(MotorState value) {
            if (target == null) {
                return;
            }

            target.ActiveState = value;
        }

        public void SetUseGUILayout(bool value) {
            if (target == null) {
                return;
            }

            target.useGUILayout = value;
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