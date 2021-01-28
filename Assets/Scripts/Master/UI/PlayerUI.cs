using System;
using System.Linq;
using Common;
using GGJ.Master.UI.Knowledge;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Misc;
using Lunari.Tsuki.Runtime.Singletons;
using Movement.States;
using Props.Interactables;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
namespace GGJ.Master.UI {
    public class PlayerUI : Singleton<PlayerUI> {
        public Clock clock = 0.25F;
        [Required]
        public KnowledgeTable table;
        public KnowledgeIndicator indicator;
        private InteractionAttachment interactionAttachment;
        private View last;
        private void Start() {
            Player.Instance.onPawnChanged.AddListener(arg0 => interactionAttachment = arg0.GetComponentInChildren<InteractionAttachment>());
        }
        private void Update() {
            var pawn = Player.Instance.Pawn;
            if (pawn == null) {
                return;
            }
            if (interactionAttachment != null) {
                if (clock.Tick()) {
                    var found = Physics2D.OverlapCircleAll(
                        pawn.transform.position,
                        interactionAttachment.radius,
                        GameConfiguration.Instance.interactableMask
                    );
                    if (found.IsNullOrEmpty()) {
                        TryHideLast();
                        return;
                    }
                    var candidates = found.Select(
                        col => col.gameObject.GetComponent<Interactable>()
                    ).Where(
                        obj => obj != null && obj.view != null
                    ).ToArray();
                    if (candidates.IsNullOrEmpty()) {
                        TryHideLast();
                        return;
                    }
                    var now = candidates.MinBy(
                        col => Vector2.Distance(pawn.transform.position, col.transform.position)
                    );
                    if (last != now.view) {
                        TryHideLast();
                    }
                    var view = now.view;
                    view.Show();
                    last = view;
                }
            }
        }
        private void TryHideLast() {
            if (last != null) {
                last.Hide();
            }
            last = null;
        }
    }
}