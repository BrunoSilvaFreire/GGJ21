using GGJ.Master.UI;
using Lunari.Tsuki.Entities;
using Props.Interactables;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
namespace GGJ.Master {
    public class Altar : Interactable {

        [ShowInInspector]
        public override void Interact(Entity entity) {
            if (!entity.Access(out Knowledgeable knowledgeable)) {
                return;
            }
            EventSystem.current.SetSelectedGameObject(PlayerUI.Instance.knowledgeFirstSelected);
        }
    }
}