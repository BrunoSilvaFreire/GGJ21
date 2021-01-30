using UnityEngine;

namespace World {
    public class Map : MonoBehaviour, ITiledMap {
        public void Setup(MapData data) {
            Vector3 mapCoordinates = Vector3.zero;
            foreach (var property in data.properties) {
                if (property.name.Equals("mapx")) {
                    mapCoordinates += new Vector3(int.Parse(property.value), 0);
                }
                else if (property.name.Equals("mapy")) {
                    mapCoordinates += new Vector3(0, int.Parse(property.value));
                }
            }
            mapCoordinates *= new Vector2(data.width, -data.height);
            transform.position += mapCoordinates;
        }
        
        public void OnEnterMap() {
            
        }

        public void OnLeaveMap() {
            
        }
    }
}