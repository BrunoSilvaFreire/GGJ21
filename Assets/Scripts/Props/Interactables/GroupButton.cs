using GGJ.Master;
using GGJ.Props;
using Lunari.Tsuki.Entities;
using World;

namespace Props.Interactables {
    
    public class GroupButton : Interactable, ITiledObject, IButtonGroup {

        private int m_buttonGroupId;
        
        public void Setup(ObjectData data) {
            m_buttonGroupId = PropertyData.GetInt(data.properties, "id");
        }

        public override void Interact(Entity entity) {
            if (entity != Player.Instance.Pawn) {
                return;
            }
            ButtonGroupManager.Instance.RemoveFromButtonGroup(m_buttonGroupId);
        }

        public void ConfigureGroup() {
            ButtonGroupManager.Instance.AddToButtonGroup(m_buttonGroupId);
        }
    }
}