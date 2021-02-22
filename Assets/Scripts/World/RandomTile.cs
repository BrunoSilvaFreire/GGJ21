using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace GGJ.World {
    [CreateAssetMenu]
    public class RandomTile : TileBase {
        public List<Sprite> sprites;
        private Tile.ColliderType colliderType;
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            var hash = 17;
            hash = hash * 31 + position.GetHashCode();
            hash = hash * 31 + tilemap.GetHashCode();
            var index = hash % sprites.Count;
            tileData.sprite = sprites[index];
            tileData.colliderType = colliderType;
            tileData.transform = Matrix4x4.identity;
        }
    }
}