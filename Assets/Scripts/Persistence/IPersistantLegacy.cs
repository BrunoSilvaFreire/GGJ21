using System;
using Lunari.Tsuki.Entities;
using UnityEngine;
using Object = UnityEngine.Object;
namespace GGJ.Persistence {
    
    public delegate void DataLoaderDelegate<T>(T value);

    public interface IPersistant {
        public void Configure(PersistenceData data);
        public int GetInstanceID();
    }

    [Obsolete]
    public interface IPersistantLegacy {
        [Obsolete]
        void ConfigurePersistance(PersistenceManager manager);
    }
}