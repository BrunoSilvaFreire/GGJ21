using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace Common {
    [Serializable]
    [BoxGroup("Bindables")]
    [HideReferenceObjectPicker]
    public class Bindable<T> {
        [SerializeField, HideInInspector]
        private T value;
        [HideReferenceObjectPicker]
        public UnityEvent onChanged = new UnityEvent();

        [ShowInInspector, PropertyOrder(int.MinValue)]
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