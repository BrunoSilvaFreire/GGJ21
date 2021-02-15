using System;
using GGJ.Common;
using GGJ.Movement;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Master.Movements {
    [TraitLocation("FX")]
    public class GroundedListener : Trait {
        public UnityEvent onLanded, onLifted;
        private readonly BooleanHistoric groundedThisFrame = new BooleanHistoric();
        private Motor motor;
        public override void Configure(TraitDependencies dependencies) {
            dependencies.DependsOn(out motor);
        }
        private void FixedUpdate() {
            groundedThisFrame.Current = motor.supportState.down;
            if (groundedThisFrame.JustActivated) {
                onLanded.Invoke();
            }
            if (groundedThisFrame.JustDeactivated) {
                onLifted.Invoke();
            }
        }
    }
}