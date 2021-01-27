using Common;
using UnityEngine.Events;
namespace Movement.States {
    public class GroundedState : MotorState {
        public float speed = 10;
        public float deceleration = 0.15F;
        public float jumpHeight;
        public float extraGravity = 10;
        private BooleanHistoric jumpedThisFrame = new BooleanHistoric();
        public UnityEvent onJumped;

        public BooleanHistoric JumpedThisFrame => jumpedThisFrame;

        public override void Tick(Motor motor) {
            Movements.Horizontal(motor, deceleration, speed);
            jumpedThisFrame.Current = Movements.Jumping(motor, jumpHeight, extraGravity);
            if (jumpedThisFrame.JustActivated) {
                onJumped.Invoke();
            }
        }
    }
}