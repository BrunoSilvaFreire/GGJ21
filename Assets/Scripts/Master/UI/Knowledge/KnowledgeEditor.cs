using System;
using System.Linq;
using FMODUnity;
using GGJ.Traits;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using Movement;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GGJ.Master.UI.Knowledge {
    public class KnowledgeEditor : UIBehaviour {
        [Required]
        public KnowledgeTable table;
        [Required]
        public KnowledgeIndicator indicator;
        private KnowledgeView toChangeFrom;
        public UnityEvent onOpened;
        private bool opened;
        public StudioEventEmitter bgmEmitter;
        public float normalBGMPhase = 0;
        public float editingBGMPhase = 1;
        public float timeUntilOpen = 5;
        private float timeLeft;
        private void Update() {
            UpdateKnowledge();
            table.group.interactable = toChangeFrom != null;
            if (opened) {
                if (Player.Instance.playerSource.GetCancel()) {
                    if (toChangeFrom == null) {
                        Close();
                    } else {
                        SelectFirstAction();
                    }
                }
            }
        }
        private void UpdateKnowledge() {
            var p = Player.Instance.Pawn;
            if (p != null && p.Access(out Motor m)) {
                var stopped = Math.Abs(m.rigidbody.velocity.x) <= 0.5F;
                if (stopped) {
                    timeLeft -= Time.deltaTime;
                } else {
                    timeLeft = timeUntilOpen;
                }
            }
            foreach (var view in indicator.Views) {
                view.Shown = timeLeft <= 0 || opened;
            }
        }
        public void Close() {
            opened = false;
            bgmEmitter.EventInstance.setParameterByName("Phase", normalBGMPhase);
            table.view.Hide();
            EventSystem.current.SetSelectedGameObject(null);
            if (Player.Instance.Access(out Motor motor)) {
                motor.Control = 1;
            }
        }
        protected override void Start() {
            indicator.onViewsAssigned.AddListener(delegate
            {
                foreach (var knowledgeView in indicator.Views) {
                    knowledgeView.button.onClick.AddListener(() =>
                    {
                        toChangeFrom = knowledgeView;
                        EventSystem.current.SetSelectedGameObject(table.Views.First().gameObject);
                    });
                }
            });

            table.onViewsAssigned.AddListener(ReloadTableListeners);
            if (table.Views != null) {
                ReloadTableListeners();
            }
        }
        private void ReloadTableListeners() {
            foreach (var knowledgeView in table.Views) {
                knowledgeView.button.onClick.AddListener(() =>
                {
                    var toRemove = toChangeFrom.Knowledge;
                    var toAdd = knowledgeView.Knowledge;
                    if (!Player.Instance.Pawn.Access(out Knowledgeable knowledgeable)) {
                        Debug.LogWarning("Unable to access Knowledgeable in player pawn");
                        return;
                    }
                    knowledgeable.CurrentKnowledge ^= toRemove;
                    knowledgeable.CurrentKnowledge |= toAdd;
                    SelectFirstAction();
                });
            }
        }
        public void Open() {
            bgmEmitter.EventInstance.setParameterByName("Phase", editingBGMPhase);
            if (opened) {
                return;
            }
            onOpened.Invoke();
            opened = true;
            table.view.Show();
            SelectFirstAction();
            if (Player.Instance.Access(out Motor motor)) {
                motor.Control = 0;
            }
        }

        private void SelectFirstAction() {
            toChangeFrom = null;
            EventSystem.current.SetSelectedGameObject(indicator.Views.First().gameObject);
        }

    }
}