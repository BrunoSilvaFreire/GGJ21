using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Entities;
using UnityEngine.Events;
namespace GGJ.Traits {
    public class TraitBind<T> where T : Trait {
        private Entity entity;
        private T current;
        public readonly UnityEvent<T> onBound = new UnityEvent<T>();
        public void OnBound(UnityAction<T> action) {
            onBound.AddListener(action);
        }
        public void Set(Entity candidate) {
            if (candidate == null) {
                entity = null;
                onBound.Invoke(null);
                return;
            }
            if (!candidate.Access(out current)) {
                return;
            }
            entity = candidate;
            onBound.Invoke(current);
        }
        public void PoolFrom(EntityEvent entityEvent) {
            entityEvent.AddListener(Set);
        }
        
    }
}