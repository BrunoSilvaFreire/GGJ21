using Lunari.Tsuki.Entities;
using UnityEngine.Events;
namespace Traits {
    public class TraitBind<T> where T : Trait {
        private Entity entity;
        private T current;
        public UnityEvent<T> onBound = new UnityEvent<T>();
        public void OnBound(UnityAction<T> action) {
            onBound.AddListener(action);
        }
        public void PoolFrom(EntityEvent entityEvent) {
            entityEvent.AddListener(delegate(Entity arg0)
            {
                if (arg0.Access(out current)) {
                    onBound.Invoke(current);
                }
            });
        }
    }
}