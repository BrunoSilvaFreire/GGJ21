using System;
using UnityEngine;
using Lunari.Tsuki.Runtime;
using UnityEngine.Tilemaps;

namespace World {

    [Serializable]
    public class MapConfig {
        public GameObject prefab;
    }
    
    [Serializable]
    public class StringMapConfigLookup : SerializableDictionary<string, MapConfig> {}


    [Serializable]
    public class LayerConfig {
        public GameObject prefab;
    }
    
    [Serializable]
    public class StringLayerConfigLookup : SerializableDictionary<string, LayerConfig> {}

    [Serializable]
    public class TileConfig {
        public Tile tile;
    }
    
    [Serializable]
    public class UintTileConfigLookup : SerializableDictionary<uint, TileConfig> {}


    [Serializable]
    public class ObjectConfig {
        public GameObject prefab;
    }
    
    [Serializable]
    public class StringObjectConfigLookup : SerializableDictionary<string, ObjectConfig> {}

    [Serializable]
    public class TileSetConfig {
        public Texture2D texture;
    }
    
    [Serializable]
    public class StringTileSetConfigLookup : SerializableDictionary<string, TileSetConfig> {}
    
    [CreateAssetMenu(fileName = "WorldGeneratorConfig", menuName = "GGJ/WorldGenerator/Config", order = 0)]
    public class WorldGeneratorConfig : ScriptableObject {
        public StringMapConfigLookup string2MapConfig;
        public StringMapConfigLookup string2LayerConfig;
        public UintTileConfigLookup uint2TileConfig;
        public StringObjectConfigLookup string2ObjectConfig;
        public StringTileSetConfigLookup string2TileSetConfig;
        public GameObject worldPrefab;
    }
}