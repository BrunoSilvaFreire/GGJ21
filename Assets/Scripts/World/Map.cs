using System;
using System.Collections.Generic;
using UnityEngine;

namespace World {
    public class Map : MonoBehaviour, ITiledMap {

        [SerializeField] private List<string> m_adjacentScenes;
        
        public Vector3 MapCoordinates { get; private set; }
        
        public void Setup(MapData data) {
            MapCoordinates = Vector3.zero;
            foreach (var property in data.properties) {
                if (property.name.Equals("mapx")) {
                    MapCoordinates += new Vector3(int.Parse(property.value), 0);
                }
                else if (property.name.Equals("mapy")) {
                    MapCoordinates += new Vector3(0, int.Parse(property.value));
                }
            }
            MapCoordinates *= new Vector2(data.width, -data.height);
            transform.position += MapCoordinates;
        }
        
        public void OnEnterMap() {
            
        }

        public void OnLeaveMap() {
            
        }
    }
}