using System.Collections;
using GGJ.Master;
using GGJ.Master.UI;
using GGJ.Traits.Combat;
using Lunari.Tsuki.Entities;
using UnityEngine;
namespace GGJ.Traits {
    public class RestartOnDeath : Trait {
        private Living living;
        public override void Configure(TraitDependencies dependencies) {
            if (dependencies.DependsOn(out living)) {
                living.onDeath.AddListener(delegate
                {
                    PersistanceManager.Instance.Restart();
                });
            }
        }
       

    }
}