using Lunari.Tsuki.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ.Master {
    public class EntityProxy : MonoBehaviour {

        [Required]
        public Entity entity;

        public UnityEvent onOpenAnimationFinished;
        
        public void Delete() {
            if (entity != null) {
                entity.gameObject.SetActive(false);
            }
        }

        public void OpenAnimationFinished() {
            onOpenAnimationFinished.Invoke();
        }
    }
}