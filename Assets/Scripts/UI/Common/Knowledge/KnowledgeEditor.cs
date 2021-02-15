using System;
using System.Linq;
using FMODUnity;
using GGJ.Common;
using GGJ.Game;
using GGJ.Game.Traits;
using GGJ.Movement;
using GGJ.UI.Common.Hints;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ.UI.Common.Knowledge {
    public class KnowledgeEditor : AnimatedView {
        [Required]
        public KnowledgeTerminal terminal;
        [Required]
        public KnowledgeIndicator indicator;
        private Bindable<KnowledgeView> toChangeFrom;
        public StudioEventEmitter bgmEmitter;
        public float normalBGMPhase;
        public float editingBGMPhase = 1;
        public float timeUntilOpen = 5;
        private float timeLeft;
        private KnowledgeView lastSelected;
        private static readonly int Marked = Animator.StringToHash("Marked");
        public Hint selectHint, backHint;
        // TODO: This is shit
        private void UpdateDependencies() {
            var selGo = EventSystem.current.currentSelectedGameObject;
            if (lastSelected != null && selGo == lastSelected.gameObject) {
                return;
            }
            Game.Knowledge selected;
            KnowledgeView selectedView = null;
            if (selGo != null) {
                selectedView = selGo.GetComponent<KnowledgeView>();
            }
            if (selectedView != null) {
                lastSelected = selectedView;
                selected = selectedView.Knowledge;
            } else {
                selected = Game.Knowledge.None;
            }
            var needed = KnowledgeDatabase.Instance.GetAllKnowledge(selected);
            foreach (var knowledgeView in terminal.Views) {
                var knowledge = knowledgeView.Knowledge;
                knowledgeView.SetShownAsDependency((needed & knowledge) == knowledge);
            }
        }

        [ShowInInspector]
        public KnowledgeView ToChangeFrom {
            get => toChangeFrom?.Value;
            set {
                if (toChangeFrom.Value != null) {
                    toChangeFrom.Value.animator.SetBool(Marked, false);
                }
                toChangeFrom.Value = value;
                if (value != null) {
                    value.animator.SetBool(Marked, true);
                }
            }
        }

        private void Update() {
            UpdateKnowledge();
            UpdateDependencies();
            terminal.group.interactable = ToChangeFrom != null;
            if (Shown) {
                if (Player.Instance.playerSource.GetCancel()) {
                    if (ToChangeFrom == null) {
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
            indicator.Shown = timeLeft <= 0 || Shown;
        }
        public void Close() {
            ToChangeFrom = null;
            Shown = false;
            terminal.view.Hide();
            EventSystem.current.SetSelectedGameObject(null);
            if (Player.Instance.Access(out Motor motor)) {
                motor.Control = 1;
                motor.entityInput.jump.overriden = false;

            }
            bgmEmitter.EventInstance.setParameterByName("Phase", normalBGMPhase);
        }
        private void Start() {
            toChangeFrom = new Bindable<KnowledgeView>();
            indicator.onViewsAssigned.AddListener(ReloadIndicatorHooks);
            terminal.onViewsAssigned.AddListener(ReloadTableListeners);
            if (terminal.Views != null) {
                ReloadTableListeners();
            }
            if (indicator.subviews != null) {
                ReloadIndicatorHooks();
            }
        }
        private void ReloadIndicatorHooks() {
            foreach (var knowledgeView in indicator.subviews.OfType<KnowledgeView>()) {
                knowledgeView.button.onClick.AddDisposableListener(() => {
                    ToChangeFrom = knowledgeView;
                    EventSystem.current.SetSelectedGameObject(terminal.Views.First().gameObject);
                }).DisposeOn(indicator.onViewsAssigned);
            }
        }
        private void ReloadTableListeners() {
            foreach (var knowledgeView in terminal.Views) {
                knowledgeView.button.onClick.AddListener(() => {
                    var toRemove = ToChangeFrom.Knowledge;
                    var toAdd = knowledgeView.Knowledge;
                    if (!Player.Instance.Pawn.Access(out Knowledgeable knowledgeable)) {
                        Debug.LogWarning("Unable to access Knowledgeable in player pawn");
                        return;
                    }
                    var value = knowledgeable.CurrentKnowledge.Value;
                    value &= ~toRemove;
                    value |= toAdd;
                    value = KnowledgeDatabase.Instance.Validate(value);
                    knowledgeable.CurrentKnowledge.Value = value;
                    SelectFirstAction();
                });
            }
        }

        public void Open() {
            if (Shown) {
                return;
            }
            onShow.Invoke();
            Show();
            terminal.view.Show();
            SelectFirstAction();
            if (Player.Instance.Access(out Motor motor)) {
                motor.Control = 0;
                motor.entityInput.jump.overriden = true;
                motor.entityInput.jump.overwriteValue = false;
            }
            bgmEmitter.EventInstance.setParameterByName("Phase", editingBGMPhase);
        }

        private void SelectFirstAction() {
            ToChangeFrom = null;

            EventSystem.current.SetSelectedGameObject(indicator.subviews.First().gameObject);
        }

    }
}