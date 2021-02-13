using System;
using UnityEngine;
using Object = UnityEngine.Object;
namespace GGJ.Persistence {
    [Serializable]
    public abstract class PersistentProperty {
        [SerializeField]
        public string key;
        public abstract PersistentProperty AsSafeProperty();
    }
    [Serializable]
    public class PersistentProperty<T> : PersistentProperty {
        [SerializeField]
        private T savedValue;
        public PersistentProperty(string key) {
            this.key = key;
        }
        public PersistentProperty(string key, T savedValue) : this(key) {
            this.savedValue = savedValue;
        }
        public static implicit operator PersistentProperty<T>(string value) => new PersistentProperty<T>(value);
        public override PersistentProperty AsSafeProperty() {
            if (savedValue == null) {
                return null;
            }
            if (savedValue is Object obj) {
                return new IntPersistentProperty(key, obj.GetInstanceID());
            }
            return this;
        }
    }
    [Serializable]
    public class IntPersistentProperty : PersistentProperty<int> {
        public IntPersistentProperty(string key) : base(key) { }
        public IntPersistentProperty(string key, int savedValue) : base(key, savedValue) { }
    }
    [Serializable]
    public class BooleanPersistentProperty : PersistentProperty<bool> {
        public BooleanPersistentProperty(string key) : base(key) { }
        public BooleanPersistentProperty(string key, bool savedValue) : base(key, savedValue) { }
    }
    [Serializable]
    public class Vector3PersistentProperty : PersistentProperty<Vector3> {
        public Vector3PersistentProperty(string key) : base(key) { }
        public Vector3PersistentProperty(string key, Vector3 savedValue) : base(key, savedValue) { }
    }
    [Serializable]
    public class Vector2PersistentProperty : PersistentProperty<Vector2> {
        public Vector2PersistentProperty(string key) : base(key) { }
        public Vector2PersistentProperty(string key, Vector2 savedValue) : base(key, savedValue) { }
    }
    
}