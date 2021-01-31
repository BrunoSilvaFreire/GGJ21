using Lunari.Tsuki.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
namespace GGJ.Master {
    public class EntityProxy : MonoBehaviour {

        [Required]
        public Entity entity;
        
        public void Delete() {
            if (entity != null) {
                entity.Delete();
            }
        }
    }
}