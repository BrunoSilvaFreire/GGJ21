using System;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;
namespace GGJ.World.Editor {

    [Serializable]
    public class LayerOptionsDictionary : SerializableDictionary<WorldLayer, LayerOptions> { }

    [Serializable]
    public struct LayerOptions {
        public GameObject palette;
        public string brushName;
    }
    
    [CreateAssetMenu]
    public class MapDatabase : ScriptableSingleton<MapDatabase> {
        public Room roomPrefab;
        public LayerOptionsDictionary options;
    }
}