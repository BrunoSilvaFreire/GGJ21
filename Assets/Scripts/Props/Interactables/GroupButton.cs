using GGJ.Master;
using GGJ.Props;
using Lunari.Tsuki.Entities;
using UnityEngine;
using World;

namespace Props.Interactables {
    
    public class GroupButton : Interactable, ITiledObject, IButtonGroup {

        [SerializeField] private int m_buttonGroupId;

        private ButtonGroupManager m_manager;
        
        public void Setup(ObjectData data) {
            m_buttonGroupId = PropertyData.GetInt(data.properties, "id");
        }

        public override void Interact(Entity entity) {
            if (entity != Player.Instance.Pawn) {
                return;
            }
            m_manager.RemoveFromButtonGroup(m_buttonGroupId);
        }

        public void ConfigureGroup(ButtonGroupManager manager) {
            m_manager = manager;
            m_manager.AddToButtonGroup(m_buttonGroupId);
        }
    }
}