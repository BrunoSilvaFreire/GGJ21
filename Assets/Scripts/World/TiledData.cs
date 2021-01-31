using System;
using System.Linq;

namespace World {
    
    [System.Serializable]
    public class PropertyData {
        public string name;
        public string type;
        public string value;
        
        public static int GetInt(PropertyData[] properties, string name) {
            return Get(properties, name, int.Parse);
        }
        
        public static float GetFloat(PropertyData[] properties, string name) {
            return Get(properties, name, float.Parse);
        }
        
        public static bool GetBool(PropertyData[] properties, string name) {
            return Get(properties, name, bool.Parse);
        }
        
        public static string GetString(PropertyData[] properties, string name) {
            return Get(properties, name, s => s);
        }

        public static T Get<T>(PropertyData[] properties, string name, Func<string, T> parser) {
            foreach (var property in properties) {
                if (property.name.Equals(name)) {
                    return parser(property.value);
                }
            }
            return default;
        } 
    }

    [System.Serializable]
    public class ObjectData {
        public PropertyData[] properties;
        public string type;
        public string name;
        public int id;
        public int x;
        public int y;
        public int width;
        public int height;
    }
        
    [System.Serializable]
    public class LayerData {
        public PropertyData[] properties;
        public uint[] data;
        public ObjectData[] objects;
        public string type;
        public string name;
        public int width;
        public int height;
    }


    [System.Serializable]
    public class TileSetData {
        public string name;
        public int columns;
        public int tilecount;
        public int tileheight;
        public int tilewidth;
        public int imageheight;
        public int imagewidth;
    }

    [System.Serializable]
    public class MapData {
        public string type;
        public int width;
        public int height;
        public LayerData[] layers;
        public PropertyData[] properties;
        public TileSetData[] tilesets;

    }

    public interface ITiledMap {
        void Setup(MapData data);
    }
    
    public interface ITiledLayer {
        void Setup(LayerData data);
    }
    
    public interface ITiledObject {
        void Setup(ObjectData data);
    }
}