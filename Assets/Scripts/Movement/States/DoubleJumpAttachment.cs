using Shiroi.FX.Effects;
namespace Movement.States {
    public class DoubleJumpAttachment : MotorState {
        private bool used;
        public float jumpHeight = 20.0F;
        public GroundedState groundedState;
        public Effect OnDoubleJump;

        public override void Tick(Motor motor) {
            if (motor.supportState.down || motor.supportState.Horizontal != 0) {
                used = false;
                return;
            }

            if (used) {
                return;
            }

            var rigidbody = motor.rigidbody;
            var velocity = rigidbody.velocity;
            if (groundedState.JumpedThisFrame) {
                if (velocity.y > 0) {
                    velocity.y += jumpHeight;
                    
                } else {
                    velocity.y = jumpHeight;
                }
                OnDoubleJump.PlayIfPresent(motor.Owner);
                used = true;
            }
            

            rigidbody.velocity = velocity;
        }
    }
}