using GGJ.UI;
using Lunari.Tsuki.Entities;
namespace GGJ.Props.Interactables {
    public abstract class Interactable : Trait {
        public View view;
        public abstract void Interact(Entity entity);
    }
}