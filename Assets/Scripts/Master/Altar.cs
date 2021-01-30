using GGJ.Master.UI;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Entities;
using Props.Interactables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
namespace GGJ.Master {
    public class Altar : Interactable {


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
            var ui = PlayerUI.Instance;
            ui.KnowledgeEditor.Open();
        }

        private void OnShow() {
            PersistanceManager.Instance.Save();
        }
    }
}