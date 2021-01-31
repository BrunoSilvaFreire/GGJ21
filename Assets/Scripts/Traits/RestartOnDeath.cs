using System.Collections;
using GGJ.Master;
using GGJ.Master.UI;
using GGJ.Traits.Combat;
using Lunari.Tsuki.Entities;
using UnityEngine;
namespace GGJ.Traits {
    public class RestartOnDeath : Trait {
        private Living living;
        public float timeUntilClose = .5F;
        public float timeUntilLoad = .5F;
        public float minWaitTime = .5F;
        public override void Configure(TraitDependencies dependencies) {
            if (dependencies.DependsOn(out living)) {
                living.onDeath.AddListener(delegate
                {
                    StartCoroutine(Restart());
                });
            }
        }
        private IEnumerator Restart() {
            var ui = PlayerUI.Instance;
            yield return new WaitForSeconds(timeUntilClose);
            var time = Time.time;
            ui.deathCurtains.Show();
            yield return new WaitForSeconds(timeUntilLoad);
            PersistanceManager.Instance.Load();
            var remaining = minWaitTime - (Time.time - time);
            if (remaining > 0) {
                yield return new WaitForSeconds(remaining);
            }
            living.Alive = true;
            ui.deathCurtains.Hide();
            yield break;
        }

    }
}