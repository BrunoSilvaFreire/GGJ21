using System;
using System.Collections.Generic;
using GGJ.Props.Collectables;
using Lunari.Tsuki.Entities;
namespace GGJ.Props.Traits {
    // TODO: Refactor this, make this work for everything that can be collected, keys, parts, etc
    public class Collector : Trait {

        public List<Collectable> Possessions {
            get;
        } = new List<Collectable>();

        public void Collect(Collectable key) {
            throw new NotImplementedException();
        }
        /*public Key key {
            get;
            set;
        }

        public void Collect(Key key) {
            this.key = key;
        }

        public void Collect(AthenaPart part) {
            GameManager.Instance.CollectAthenaPart(part.id);
        }

        public void Collect(MemorySlot memorySlot) {
            Owner.GetTrait<Knowledgeable>().MaxNumberOfKnowledge.Value++;
            memorySlot.gameObject.SetActive(false);
        }*/


        public T FindCollectable<T>() where T : class {
            foreach (var collectable in Possessions) {
                if (collectable is T found) {
                    return found;
                }
            }
            return null;
        }
        public void Remove(Collectable collectable) {
            Possessions.Remove(collectable);
        }
    }
}