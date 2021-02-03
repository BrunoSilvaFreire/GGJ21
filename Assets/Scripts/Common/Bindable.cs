using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace Common {
    [Serializable]
    public class Bindable<T> {
        [SerializeField, HideInInspector]
        private T value;
        public UnityEvent onChanged = new UnityEvent();

        [ShowInInspector]
        public T Value {
            get => value;
            set {
                if (value.Equals(this.value)) {
                    return;
                }
                this.value = value;
                onChanged.Invoke();
            }
        }

        public static implicit operator T(Bindable<T> bindable) {
            return bindable.value;
        }
        public void Bind(UnityAction<T> listener) {
            onChanged.AddListener(delegate {
                listener(value);
            });
            listener(value);
        }
    }
}