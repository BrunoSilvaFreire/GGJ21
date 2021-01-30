using System;
using DG.Tweening;
using GGJ.Traits;
using Lunari.Tsuki.Entities;
using Props.Collectables;
using UnityEngine;

namespace Props.Interactables {
    public class ElectricDoor : Trait {
        
        private AnimatorBinder m_binder;
        
        public override void Configure(TraitDependencies dependencies) {
            dependencies.DependsOn(out m_binder);
        }

        public void Open(Key key) {
            m_binder.Animator.SetTrigger("open");
            transform.DOScale(Vector3.zero, 1f).OnComplete(() => gameObject.SetActive(false));
            key.Consume();
        }
    }
}