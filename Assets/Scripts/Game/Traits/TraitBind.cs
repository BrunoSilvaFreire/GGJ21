using System;
using Common;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Traits {

    public class TraitBind<T> where T : Trait {
        private T current;
        public readonly UnityEvent<T> onBound = new UnityEvent<T>();

        public Entity Entity {
            get;
            private set;
        }

        public T Current => current;

        public void Bind(UnityAction<T> action) {
            onBound.AddListener(action);
        }
        public void Set(Entity candidate) {
            if (candidate == null) {
                Entity = null;
                onBound.Invoke(null);
                return;
            }
            if (!candidate.Access(out current)) {
                return;
            }
            Entity = candidate;
            onBound.Invoke(current);
        }
        public void PoolFrom(EntityEvent entityEvent) {
            entityEvent.AddListener(Set);
        }
        public void BindToValue<A>(Func<T, A> value, Func<T, UnityEvent> onChanged, UnityAction<A> action) {
            onBound.AddListener(arg0 => {
                var uEvent = onChanged(arg0);
                uEvent.AddDisposableListener(delegate {
                    action(value(arg0));
                }).DisposeOn(onBound);
            });
            if (current != null) {
                action(value(current));
            }
        }
        public void BindToValue<A>(Func<T, Bindable<A>> bindable, UnityAction<A> action) {
            BindToValue(
                trait => bindable(trait).Value,
                trait => bindable(trait).onChanged,
                action
            );
        }


    }
}