using System;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.Events;
namespace Props.Collectables {
    public abstract class Collectable : MonoBehaviour {
        public UnityEvent onCollected;

        public enum CollectionAction {
            None,
            Delete
        }

        private void OnTriggerEnter2D(Collider2D other) {
            var e = other.GetComponent<Entity>();
            if (e != null) {
                Collect(e);
            }
        }

        public void Collect(Entity entity) {
            var result = ProcessCollection(entity);
            if (result != CollectionAction.None) {
                onCollected.Invoke();
            }

            switch (result) {
                case CollectionAction.None:
                    break;
                case CollectionAction.Delete:
                    Destroy(gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected abstract CollectionAction ProcessCollection(Entity entity);
    }
}