using System;
using GGJ.Traits.Combat;
using Lunari.Tsuki.Entities;
using Props.Collectables;
using Props.Interactables;
using UnityEngine;

namespace GGJ.Traits {
    public class Collector : Trait {

        private Key m_key;
        
        public void Collect(Key key) {
            m_key = key;
            m_key.transform.parent = transform.parent;
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (IsElectrictDoor(collision.collider, out var door)) {
                if (m_key) {
                    door.Open(m_key);
                    m_key = null;
                }
                else {
                    Owner.GetTrait<Living>().Kill();
                }
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