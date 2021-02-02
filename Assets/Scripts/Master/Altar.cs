using GGJ.Master.UI;
using GGJ.Traits;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Entities;
using Movement;
using Props.Interactables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
namespace GGJ.Master {
    public class Altar : Interactable, IPersistant {

        private PersistanceManager m_manager;
        public const string RespawnName = "Respawn";
        private static readonly int Respawn = Animator.StringToHash(RespawnName);

        private void OnEnable() {
            view.onShow.AddListener(OnShow);
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
            ui.KnowledgeEditor.Open();
        }

        private void OnShow() {
            m_manager.Save();
        }

        public void ConfigurePersistance(PersistanceManager manager) {
            m_manager = manager;
            manager.onLoad.AddListener(delegate
            {
                if (Owner.Access(out AnimatorBinder binder)) {
                    binder.Animator.SetTrigger(Respawn);
                }
            });
        }
    }
}