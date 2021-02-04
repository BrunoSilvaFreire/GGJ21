using System;
using GGJ.Collectables;
using GGJ.Master;
using GGJ.Props.Collectables;
using GGJ.Traits.Combat;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Entities;
using Props.Collectables;
using Props.Interactables;
using UnityEngine;

namespace GGJ.Traits {
    public class Collector : Trait {

        private Key m_key;
        
        public void Collect(Key key) {
            m_key = key;
        }

        public void Collect(AthenaPart part) {
            GameManager.Instance.CollectAthenaPart(part.id);
        }

        public void Collect(MemorySlot memorySlot) {
            Owner.GetTrait<Knowledgeable>().MaxNumberOfKnowledge.Value++;
            memorySlot.gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (IsElectrictDoor(collision.collider, out var door)) {
                door.Open(ref m_key);
            }
        }
        
        private static bool IsElectrictDoor(Collider2D other, out ElectricDoor door) {
            var entity = other.GetComponentInParent<Entity>();
            if (entity == null) {
                door = null;
                return false;
            }
            return entity.Access(out door);
        }
    }
}