using System;
using GGJ.World;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace World {
    [Flags]
    public enum WorldLayer {
        Background = 1 << 0,
        WorldStatic = 1 << 1,
        Objects = 1 << 2,
        Hazards = 1 << 3,
        Foreground = 1 << 4
    }
    [Serializable]
    public class LayerToTilemap : SerializableDictionary<WorldLayer, Tilemap> { }
    
    [Serializable]
    public class RoomGroupDictionary : SerializableDictionary<Room, string> { }
    
    
    public class WorldManager : Singleton<WorldManager> {
        public Vector2Int roomSize;
        public Grid grid;
        public LayerToTilemap tilemap;
        public RoomGroupDictionary groups;

        public Vector2Int RoomCoordinates(Room room) {
            var pos = room.transform.position;
            var result = new Vector2Int {
                x = Mathf.RoundToInt(pos.x / roomSize.x),
                y = Mathf.RoundToInt(pos.y / roomSize.y)
            };
            return result;
        }
        public Vector2 LocalToWorld(Vector2Int room) => new Vector2(
            room.x * roomSize.x,
            room.y * roomSize.y
        );
    }
}