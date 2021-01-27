using System.Collections.Generic;
using Lunari.Tsuki.Runtime;
using UnityEngine;
namespace Common.Pooling {
    public abstract class Pool<T> : MonoBehaviour where T : Component {
        public T prefab;
        public byte preWarm;
        private Stack<T> pooled;

        private void Start() {
            pooled = new Stack<T>();
            for (byte i = 0; i < preWarm; i++) {
                var obj = Instantiate(prefab, transform, true);
                pooled.Push(obj);
                obj.gameObject.SetActive(false);
            }
        }

        public void Return(T value) {
            value.gameObject.SetActive(false);
            value.transform.SetParent(transform);
            pooled.Push(value);
        }

        public T Get() {
            var selected = pooled.IsEmpty() ? null : pooled.Pop();
            if (selected != null) {
                selected.gameObject.SetActive(true);
                selected.transform.SetParent(null);
            }

            return selected;
        }
    }
}