using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace World {
    public class Map : MonoBehaviour, ITiledMap {

        [SerializeField] private Vector2Int m_coordinates;
        public Vector2Int Coordinates => m_coordinates;

        private bool m_playerInside = false;

        public void Setup(MapData data) {
            m_coordinates = Vector2Int.zero;
            foreach (var property in data.properties) {
                if (property.name.Equals("mapx")) {
                    m_coordinates += new Vector2Int(int.Parse(property.value), 0);
                }
                else if (property.name.Equals("mapy")) {
                    m_coordinates += new Vector2Int(0, int.Parse(property.value));
                }
            }
            transform.position += new Vector3(Coordinates.x * data.width, Coordinates.y * -data.height);
        }

        private void Awake() {
            MapManager.Instance.AddMap(this);
        }

        public void Activate() {
            gameObject.SetActive(true);
        }

        public void Deactivate() {
            gameObject.SetActive(false);
        }
        
        
        public void OnEnterMap() {
            if (m_playerInside) {
                return;
            }
            m_playerInside = true;
            MapManager.Instance.SetActiveMap(this);
        }

        public void OnLeaveMap() {
            m_playerInside = false;
        }
    }
}