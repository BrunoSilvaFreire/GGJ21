using GGJ.Collectables;
using GGJ.Master;
using GGJ.Props.Collectables;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Entities;
using Props.Collectables;
using Props.Interactables;
using UnityEngine;

namespace GGJ.Traits {
    // TODO: Refactor this, make this work for everything that can be collected, keys, parts, etc
    public class Collector : Trait {

        public Key key {
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
        }


    }
}