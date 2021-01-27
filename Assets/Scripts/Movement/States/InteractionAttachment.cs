using Common;
using Props.Interactables;
using UnityEngine;
namespace Movement.States {
    public class InteractionAttachment : MotorState {
        public float radius = 2;

        public override void Tick(Motor motor) {
            if (!motor.entityInput.interacting.Discrete.Consume()) {
                return;
            }

            var found = Physics2D.OverlapCircle(motor.transform.position, radius,
                GameConfiguration.Instance.interactableMask);
            if (found == null) {
                return;
            }
            var interactable = found.GetComponentInChildren<Interactable>();
            if (interactable != null) {
                interactable.Interact(motor.Owner);
            }
        }
    }
}