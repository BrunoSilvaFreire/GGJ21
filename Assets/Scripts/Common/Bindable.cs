using System;
using Lunari.Tsuki.Runtime;
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
                if (value != null && value.Equals(this.value)) {
                    return;
                }
                this.value = value;
                onChanged.Invoke();
            }
        }

        public static implicit operator T(Bindable<T> bindable) {
            return bindable.value;
        }
        public DisposableListener Bind(UnityAction<T> listener) {
            var lis = onChanged.AddDisposableListener(() => listener(value));
            listener(value);
            return lis;
        }
    }
}