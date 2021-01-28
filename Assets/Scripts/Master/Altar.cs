using GGJ.Master.UI;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Entities;
using Props.Interactables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
namespace GGJ.Master {
    public class Altar : Interactable {

        [ShowInInspector]
        public override void Interact(Entity entity) {
            if (!entity.Access(out Knowledgeable _)) {
                return;
            }
            var ui = PlayerUI.Instance;
            var table = ui.table;
            table.view.Show();
            var selected = table.transform.GetChild(0).gameObject;
            Debug.Log(selected);
            EventSystem.current.SetSelectedGameObject(selected);
        }
    }
}