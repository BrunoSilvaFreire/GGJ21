using System;
using Lunari.Tsuki.Entities;
using UnityEngine;
namespace GGJ.Traits {
    public class RepositionOnReload : Trait {
        private Vector3 position;
        public override void Configure(TraitDependencies dependencies) {
            position = dependencies.Entity.transform.position;
        }
        private void Awake() {
            Debug.Log("lol");
        }
        private void OnEnable() {
            if (Owner != null) {
                Owner.transform.position = position;
            }
        }
        private void OnDisable() {
            Debug.Log("dISABLE");
        }
    }
}