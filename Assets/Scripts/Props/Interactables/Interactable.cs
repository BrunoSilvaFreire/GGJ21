using Lunari.Tsuki.Entities;
using UnityEngine;
namespace Props.Interactables {
    public abstract class Interactable : Trait {
        public abstract void Interact(Entity entity);
    }
}