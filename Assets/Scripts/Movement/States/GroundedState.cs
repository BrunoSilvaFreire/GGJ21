using Common;
using Lunari.Tsuki.Runtime.Stacking;
using UnityEngine.Events;
namespace Movement.States {
    public class GroundedState : MotorState {
        public float speed = 10;
        public float deceleration = 0.15F;
        public FloatStackable jumpHeight;
        public float extraGravity = 10;
        private readonly BooleanHistoric jumpedThisFrame = new BooleanHistoric();
        public UnityEvent onJumped;

        public BooleanHistoric JumpedThisFrame => jumpedThisFrame;

        public override void Tick(Motor motor) {

            Movements.Horizontal(motor, deceleration, speed);
            jumpedThisFrame.Current = Movements.Jumping(motor, jumpHeight, extraGravity);
            if (jumpedThisFrame.JustActivated && motor.IsJumpEligible()) {
                onJumped.Invoke();
            }
           
        }
    }
}