using Common;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Movement {
    public class MotorController : MonoBehaviour {
        public float control;
        public bool overwriteControl;
        [Required]
        public Motor motor;
        private BooleanHistoric historic;

        private void Start() {
            historic = new BooleanHistoric();
        }

        private void Update() {
            historic.Current = overwriteControl;
            if (historic.Current) {
                motor.Control = control;
            }
            if (historic.JustDeactivated) {
                motor.Control = 1;

            }
        }
    }
}