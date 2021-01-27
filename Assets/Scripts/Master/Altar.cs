using Lunari.Tsuki.Entities;
using Props.Interactables;
namespace GGJ.Master {
    public class Altar : Interactable {

        public override void Interact(Entity entity) {
            if (!entity.Access(out Knowledgeable knowledgeable)) {
                return;
            }
            
        }
    }
}