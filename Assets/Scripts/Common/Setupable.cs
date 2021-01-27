using UnityEngine;
namespace Common {
    public abstract class Setupable<T> : MonoBehaviour {
        public abstract void Setup(T obj);
    }
}