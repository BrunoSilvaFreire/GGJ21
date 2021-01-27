using UnityEngine;
namespace Common {
    public interface ISetupable<T> {
        void Setup(T obj);
    }
}