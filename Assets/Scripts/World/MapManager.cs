using System;
using System.Collections.Generic;
using Lunari.Tsuki.Graphs;
using Lunari.Tsuki.Runtime.Singletons;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace World {
    
    public class MapManager : Singleton<MapManager> {

        private Dictionary<Vector2Int, Map> m_maps = new Dictionary<Vector2Int, Map>();
        
        [SerializeField] private int m_depthToActivate = 3;

        private void Start() {
            // SetActiveMap(m_initialMap);
        }
        
        public void AddMap(Map map) {
            m_maps[map.Coordinates] = map;
        }
        
        public void SetActiveMap(Map map) {
            m_maps.ForEach(kv => {
                if (kv.Value != map) {
                    kv.Value.Deactivate();    
                }
            });
            map.Activate();
            ActivateConnections(GetConnections(map.Coordinates), m_depthToActivate);
        }

        private void ActivateConnections(List<Map> connections, int depth) {
            if (depth <= 0) {
                return;
            }
            foreach (var connection in connections) {
                connection.Activate();
                ActivateConnections(GetConnections(connection.Coordinates), --depth);
            }
        }

        public List<Map> GetConnections(int x, int y) {
            return GetConnections(new Vector2Int(x, y));
        }

        public List<Map> GetConnections(Vector2Int position) {

            if (!m_maps.ContainsKey(position)) {
                Debug.LogWarningFormat("Invalid position for GetConnections: {0}", position);
                return new List<Map>();
            }

            var positions = new List<Vector2Int>
            {
                new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1)
                
            };
            var connections = new List<Map>();

            for (int i = 0; i < positions.Count; i++) {
                Vector2Int connectionPosition = position + positions[i];
                if (m_maps.ContainsKey(connectionPosition)) {
                    connections.Add(m_maps[connectionPosition]);
                }
            }

            return connections;
        }
    }
}