using System.Collections;
using GGJ.Master;
using GGJ.Master.UI;
using GGJ.Traits.Combat;
using Input;
using Lunari.Tsuki.Entities;
using Movement;
using UnityEngine;
namespace GGJ.Traits {
    public class RestartOnDeath : Trait {
        private Living living;
        public float slowDownDuration;
        public float throwForce;
        public AnimationCurve timeScale;
        private Coroutine routine;
        public override void Configure(TraitDependencies dependencies) {
            if (dependencies.DependsOn(out living)) {
                living.onDeath.AddListener(delegate(Entity killer)
                {
                    if (routine != null) {
                        return;
                    }
                    routine = StartCoroutine(PrepareRestart(killer));
                });
            }
        }
        private IEnumerator PrepareRestart(Entity killer) {
            OverridableInputSource source = null;
            if (Owner.Access(out EntityInput input)) {
                source = input.source as OverridableInputSource;
                if (source != null) {
                    source.overriden = OverridableInputSource.InputOverrideFlags.Horizontal | OverridableInputSource.InputOverrideFlags.Jump;
                    source.horizontal = 0;
                    source.jump = false;
                }
            }
            Vector2 throwVec;
            if (killer != null) {
                throwVec = transform.position - killer.transform.position;
            } else {
                throwVec = new Vector2(Random.value, 1);
            }
            throwVec.Normalize();
            throwVec *= throwForce;
            Motor motor;
            float old;
            if (Owner.Access(out motor)) {
                old = motor.maxSpeed.BaseValue;
                motor.rigidbody.velocity = throwVec;
                motor.maxSpeed.BaseValue = 500;
            } else {
                old = 0;
            }
            var time = slowDownDuration;
            while (time > 0) {
                time -= Time.unscaledDeltaTime;
                Time.timeScale = timeScale.Evaluate(time / slowDownDuration);
                yield return null;
            }
            if (source != null) {
                source.overriden = 0;
            }
            if (motor != null) {
                motor.maxSpeed.BaseValue = old;
            }
            PersistanceManager.Instance.Restart();
            routine = null;
        }


    }
}