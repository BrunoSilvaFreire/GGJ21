using System.Collections;
using Febucci.UI;
using GGJ.Game;
using GGJ.Game.Traits;
using GGJ.Movement;
using GGJ.Persistence;
using GGJ.Props.Interactables;
using GGJ.Traits;
using GGJ.Traits.Animation;
using GGJ.Traits.Burns;
using GGJ.UI;
using GGJ.UI.Common;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ.Master {
    public class Altar : Interactable, IPersistantLegacy {

        private PersistenceManager m_manager;
        public const string RespawnName = "Respawn";
        private static readonly int Respawn = Animator.StringToHash(RespawnName);
        [Required]
        public TextAnimatorPlayer player;
        public float hangTime = 5, beforeShowing = 3;
        private Knowledge last;
        private Coroutine shownRoutine;
        [Required]
        public View spiceView;
        public UnityEvent onRoast;
        private Knowledge now;
        private void OnEnable() {
            view.onShow.AddListener(OnShow);

        }
        private void Start() {
            player.onTextShowed.AddListener(delegate {
                Coroutines.ReplaceCoroutine(ref shownRoutine, this, spiceView.SetShownInSeconds(false, hangTime));
            });
            view.onShow.AddListener(delegate {
                var burn = SpiceDatabase.Instance.SelectProximity();
                Burn(burn);
    
            });
            Player.Instance.Bind<Knowledgeable>().BindToValue(
                knowledgeable => knowledgeable.CurrentKnowledge,
                arg0 => {
                    last = now;
                    now = arg0;
                });
            var ui = PlayerUI.Instance;
            ui.knowledgeEditor.onHide.AddListener(delegate {
                if (last != now) {
                    var burn = SpiceDatabase.Instance.SelectKnowledge();
                    Coroutines.ReplaceCoroutine(ref shownRoutine, this, ShowRoutine(burn));    
                }
            });
        }
        private void OnDisable() {
            view.onShow.RemoveListener(OnShow);
        }
        public override void Configure(TraitDependencies dependencies) {
            if (dependencies.Access(out AnimatorBinder binder)) {
                dependencies.RequiresAnimatorParameter(RespawnName, AnimatorControllerParameterType.Trigger);
            }
        }
        [ShowInInspector]
        public override void Interact(Entity entity) {
            if (!entity.Access(out Knowledgeable _)) {
                return;
            }
            if (entity.Access(out Motor motor)) {
                if (!motor.supportState.down) {
                    return;
                }
            }
            var ui = PlayerUI.Instance;
            ui.knowledgeEditor.Open();
        }

        private void OnShow() {
            // TODO: Should this really be invoked by the UI callback?
            PersistenceManager.Instance.Save();
        }

        public void ConfigurePersistance(PersistenceManager manager) {
            m_manager = manager;
            manager.onLoad.AddListener(delegate {
                var burn = SpiceDatabase.Instance.SelectRespawn();
                Coroutines.ReplaceCoroutine(ref shownRoutine, this, ShowRoutine(burn));
                if (Owner.Access(out AnimatorBinder binder)) {
                    binder.Animator.SetTrigger(Respawn);
                }
            });
        }
        public void Burn(Burn burn) {
            onRoast.Invoke();
            spiceView.Show();
            player.ShowText(burn.message);
        }
        private IEnumerator ShowRoutine(Burn burn) {
            yield return spiceView.SetShownInSeconds(true, beforeShowing);
            Burn(burn);
        }
    }
}