using GGJ.Common;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Movement.States {
    public class GlideAttachment : MotorState {
        [Range(0, 30)]
        public float fallDownSpeed;
        public float speedReduction = 5;
        public BooleanHistoric gliding = new BooleanHistoric();
        public StudioEventEmitter glideLoop;
        public UnityEvent onBeginGlide, onEndGlide;
        public override void Begin(Motor motor) { }
        public override void Tick(Motor motor) {

            if (motor.entityInput.jump.Continuous) {

                var vel = motor.rigidbody.velocity;
                var target = -fallDownSpeed;
                if (vel.y < 0 && vel.y < target) {
                    vel.y = Mathf.Lerp(vel.y, target, speedReduction);
                    gliding.Current = true;
                } else {
                    gliding.Current = false;
                }
                motor.rigidbody.velocity = vel;
            } else {
                gliding.Current = false;
            }
            if (gliding.JustActivated) {
                onBeginGlide.Invoke();
            }
            if (gliding.JustDeactivated) {
                onEndGlide.Invoke();
            }
            var current = glideLoop.IsPlaying();
            if (gliding.Current != current) {
                if (gliding.Current) {
                    glideLoop.Play();
                } else {
                    glideLoop.Stop();
                }
            }
        }
    }
}