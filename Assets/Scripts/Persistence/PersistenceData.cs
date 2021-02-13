using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Lunari.Tsuki.Runtime.Exceptions;
using Object = UnityEngine.Object;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
namespace GGJ.Persistence {
    public class PersistenceData {
        private PersistenceManager manager;
        public int id;
        public PersistenceData(int id, PersistenceManager manager) {
            this.id = id;
            this.manager = manager;
        }

        public List<PersistentProperty> Properties {
            get;
        } = new List<PersistentProperty>();

        public void Bind<T>(string key, out PersistentProperty<T> property, DataLoaderDelegate<T> onLoaded) {
            property = CreateProperty<T>(key);
            Properties.Add(property);
            // manager.onSave.AddListener(delegate { });
            // manager.onLoad.AddListener(delegate { });
        }
        private static readonly Dictionary<Type, Func<string, PersistentProperty>> factories = new Dictionary<Type, Func<string, PersistentProperty>> {
            {
                typeof(int),
                s => new IntPersistentProperty(s)
            }, {
                typeof(Vector2),
                s => new Vector2PersistentProperty(s)
            }, {
                typeof(Vector3),
                s => new Vector3PersistentProperty(s)
            }, {
                typeof(bool),
                s => new BooleanPersistentProperty(s)
            }
        };
        private static PersistentProperty<T> CreateProperty<T>(string key) {
            var type = typeof(T);
            if (typeof(Object).IsAssignableFrom(type)) {
                return new PersistentProperty<T>(key);
            }
            if (!factories.TryGetValue(type, out var factory)) {
                throw new WTFException($"Unable to find factory for type {type} ({string.Join(", ", factories.Keys.Select(type1 => type1.ToString()))})");
            }
            return (PersistentProperty<T>)factory.Invoke(key);
        }
    }
}