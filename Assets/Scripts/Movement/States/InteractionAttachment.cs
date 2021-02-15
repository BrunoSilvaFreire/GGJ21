using GGJ.Props.Traits;
namespace GGJ.Movement.States {
    public class InteractionAttachment : MotorState {
        public float radius = 2;
        private Interactor interactor;
        public override void Begin(Motor motor) {
            interactor = motor.Owner.GetTrait<Interactor>();
        }
        public override void Tick(Motor motor) {
            if (!motor.entityInput.interacting.Discrete.Consume()) {
                return;
            }
            if (interactor == null) {
                return;
            }
            var interactable = interactor.CurrentlyInteractable;
            if (interactable != null) {
                interactable.Interact(motor.Owner);
            }
        }
    }
}