using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace World {
    public class Map : MonoBehaviour, ITiledMap {

        [SerializeField] private List<string> m_adjacentScenes;
        
        public Vector2Int Coordinates { get; private set; }
        
        public void Setup(MapData data) {
            Coordinates = Vector2Int.zero;
            foreach (var property in data.properties) {
                if (property.name.Equals("mapx")) {
                    Coordinates += new Vector2Int(int.Parse(property.value), 0);
                }
                else if (property.name.Equals("mapy")) {
                    Coordinates += new Vector2Int(0, int.Parse(property.value));
                }
            }
            MapConnectionConfig.Instance.AddMap(SceneManager.GetActiveScene(), Coordinates);
            transform.position += new Vector3(Coordinates.x * data.width, Coordinates.y * -data.height);
        }
        
        public void OnEnterMap() {
            MapManager.Instance.SetActiveMap(this);
        }

        public void OnLeaveMap() {
            
        }
    }
}