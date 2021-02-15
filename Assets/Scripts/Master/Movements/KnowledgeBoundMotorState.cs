using GGJ.Game;
using GGJ.Game.Traits;
using GGJ.Movement;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Exceptions;
using UnityEngine;
namespace GGJ.Master.Movements {
    public class KnowledgeBoundMotorState : MonoBehaviour {
        public MotorState state;
        public Knowledge knowledge;
        private Entity self;
        private void Start() {
            if (!this.TryGetComponentInParent(out self)) {
                throw new WTFException("No entity found in parent.");
            }
            if (!self.Access(out Knowledgeable knowledgeable)) {
                throw new WTFException($"Entity {self.name} is not Knowledgeable.");
            }
            knowledgeable.Bind(knowledge, hasKnowledge => state.enabled = hasKnowledge);
        }
    }
}