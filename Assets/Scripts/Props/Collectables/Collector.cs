using System;
using Lunari.Tsuki.Entities;
using Props.Collectables;
using Props.Interactables;
using UnityEngine;

namespace GGJ.Traits {
    public class Collector : Trait {

        private static bool IsElectrictDoor(Collider2D other, out ElectricDoor door) {
            var entity = other.GetComponentInParent<Entity>();
            if (entity == null) {
                door = null;
                return false;
            }
            return entity.Access(out door);
        }

        private Key m_key;
        public void Collect(Key key) {
            m_key = key;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (IsElectrictDoor(other, out var door)) {
                if (m_key) {
                    door.Open(m_key);
                }
                else {
                    //TODO kill entity
                }
            }
        }
    }
}