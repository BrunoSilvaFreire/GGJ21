using Lunari.Tsuki.Entities;
using UnityEngine;
namespace Props.Interactables {
    public abstract class Interactable : MonoBehaviour {
        public abstract void Interact(Entity entity);
    }
}