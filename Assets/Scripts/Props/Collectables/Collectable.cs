using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Props.Collectables {
    public abstract class Collectable : MonoBehaviour {
        public UnityEvent onCollected;
        protected bool collected;
        public enum CollectionAction {
            None,
            Ok,
            Delete
        }

        private void OnTriggerEnter2D(Collider2D other) {
            var e = other.GetComponent<Entity>();
            if (e != null) {
                Collect(e);
            }
        }

        public void Collect(Entity entity) {
            if (collected) {
                return;
            }
            var result = ProcessCollection(entity);
            if (result != CollectionAction.None) {
                onCollected.Invoke();
            }
            if (result != CollectionAction.None) {
                collected = true;
            }
            switch (result) {
                case CollectionAction.None:
                    break;
                case CollectionAction.Delete:
                    Destroy(gameObject);
                    break;
            }
        }

        protected abstract CollectionAction ProcessCollection(Entity entity);
    }
}