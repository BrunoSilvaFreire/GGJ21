using System;
using System.Linq;
using GGJ.Common;
using GGJ.Game;
using GGJ.Props.Interactables;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Misc;
using UnityEngine;
namespace GGJ.Props.Traits {
    [TraitLocation("Misc")]
    public class Interactor : Trait {
        public Clock clock = 0.25F;
        public float radius = .5F;
        public int maxInteractableAtOnce = 3;
        private Collider2D[] raycastBuffer;
        private Interactable[] interactables;
        private Interactable currentlyInteractable;

        public Interactable[] Interactables => interactables;

        public Interactable CurrentlyInteractable => currentlyInteractable;

        private void Start() {
            raycastBuffer = new Collider2D[maxInteractableAtOnce];
            interactables = new Interactable[maxInteractableAtOnce];
        }
        private void Update() {
            if (clock.Tick()) {
                var pawn = Player.Instance.Pawn;
                if (pawn == null) {
                    return;
                }
                var size = Physics2D.OverlapCircleNonAlloc(pawn.transform.position, radius, raycastBuffer, GameConfiguration.Instance.interactableMask);
                if (size == 0) {
                    TrySetShown(false);
                    return;
                }
                var min = float.MaxValue;
                currentlyInteractable = null;
                for (var i = 0; i < size; i++) {
                    var interactable = raycastBuffer[i].gameObject.GetComponent<Interactable>();
                    if (interactable == null) {
                        continue;
                    }

                    var attempt = Vector2.Distance(pawn.transform.position, interactable.transform.position);
                    if (attempt <= min) {
                        currentlyInteractable = interactable;
                        min = attempt;
                    }
                }
                TrySetShown(true);
            }
        }
        private void TrySetShown(bool shown) {
            if (currentlyInteractable == null) {
                return;
            }
            var view = currentlyInteractable.view;
            if (view == null) {
                return;
            }
            view.Shown = shown;
        }
    }

}