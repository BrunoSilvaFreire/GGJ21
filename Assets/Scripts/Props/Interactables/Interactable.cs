using Lunari.Tsuki.Entities;
using UI;
using UnityEngine;
namespace Props.Interactables {
    public abstract class Interactable : Trait {
        public View view;
        public abstract void Interact(Entity entity);
    }
}