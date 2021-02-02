using GGJ.Master.UI;
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

        private void OnEnable() {
            view.onShow.AddListener(OnShow);
        }

        private void OnDisable() {
            view.onShow.RemoveListener(OnShow);
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
        }
    }
}