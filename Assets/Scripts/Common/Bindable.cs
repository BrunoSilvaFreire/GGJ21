using UnityEngine.Events;
namespace Common {
    public class Bindable<T> {
        public readonly UnityEvent<T> onAttached = new UnityEvent<T>();
        public readonly UnityEvent<T> onDetached = new UnityEvent<T>();
        public Bindable(UnityEvent<T> onChanged) {
            onChanged.AddListener(delegate(T arg0) {
                if (Value != null) {
                    onDetached.Invoke(Value);
                }
                Value = arg0;
                if (arg0 != null) {
                    onAttached.Invoke(arg0);
                }
            });
        }

        public T Value {
            get;
            private set;
        }

        public static implicit operator bool(Bindable<T> bindable) {
            return bindable.Value != null;
        }
        public bool Use(out T o) {
            return (o = Value) != null;
        }
        public void WithAttachments(UnityAction<T> attached, UnityAction<T> detached) {
            onAttached.AddListener(attached);
            onDetached.AddListener(detached);
        }
    }
}