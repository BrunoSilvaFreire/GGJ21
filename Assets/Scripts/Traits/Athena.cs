using System;
using System.Collections;
using System.Collections.Generic;
using GGJ.Master;
using Lunari.Tsuki.Entities;
using UnityEngine;

namespace GGJ.Traits {
    public class Athena : Trait {

        private AnimatorBinder binder;
        public void MarkForStop() {
            binder.Animator.enabled = false;
        }
        public override void Configure(TraitDependencies dependencies) {
            dependencies.DependsOn(out binder);
        }
        
        private static bool IsApollo(Collider2D other) {
            var entity = other.GetComponentInParent<Entity>();
            if (entity == null) {
                return false;
            }
            return entity == Player.Instance.Pawn;
        } 

        private void OnTriggerEnter2D(Collider2D other) {
            if (IsApollo(other)) {
                OnApolloEnterRoom();
            }
        }

        private void OnApolloEnterRoom() {
            if (true) {
                //binder.Animator.SetTrigger("happy");
                FinalCutsceneTrigger.Instance.Play();
            }
        }
    }
}
