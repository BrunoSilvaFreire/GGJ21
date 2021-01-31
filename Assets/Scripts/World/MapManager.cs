using System;
using System.Collections.Generic;
using Lunari.Tsuki.Graphs;
using Lunari.Tsuki.Runtime.Singletons;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace World {
    
    public class MapManager : Singleton<MapManager> {
        
        [SerializeField] private int m_depthToActivate = 3;
        
        private Dictionary<Vector2Int, Map> m_maps = new Dictionary<Vector2Int, Map>();
        private Map m_activeMap;

        public void AddMap(Map map) {
            m_maps[map.Coordinates] = map;
        }

        public void SetActiveMap(Vector2Int coordinates) {
            SetActiveMap(m_maps[coordinates]);
        }
        
        public void SetActiveMap(Map map) {
            m_activeMap = map;
            m_maps.ForEach(kv => {
                if (kv.Value != map) {
                    kv.Value.Deactivate();    
                }
            });
            map.Activate();
            HashSet<Vector2Int> activatedMaps = new HashSet<Vector2Int> {map.Coordinates};
            ActivateConnections(GetConnections(map.Coordinates, activatedMaps), m_depthToActivate, activatedMaps);
        }

        private void ActivateConnections(List<Map> connections, int depth, HashSet<Vector2Int> activatedMaps) {
            if (depth <= 0) {
                return;
            }
            foreach (var connection in connections) {
                connection.Activate();
                activatedMaps.Add(connection.Coordinates);
                ActivateConnections(GetConnections(connection.Coordinates, activatedMaps), --depth, activatedMaps);
            }
        }

        public Map GetActiveMap() {
            return m_activeMap;
        }
        
        public List<Map> GetConnections(Vector2Int position, HashSet<Vector2Int> visitedMaps) {

            if (!m_maps.ContainsKey(position)) {
                Debug.LogWarningFormat("Invalid position for GetConnections: {0}", position);
                return new List<Map>();
            }

            var positions = new List<Vector2Int>
            {
                Vector2Int.down,
                Vector2Int.up,
                Vector2Int.left,
                Vector2Int.right,
                Vector2Int.right + Vector2Int.up,
                Vector2Int.right + Vector2Int.down,
                Vector2Int.left + Vector2Int.up,
                Vector2Int.left + Vector2Int.down,
            };
            var connections = new List<Map>();

            for (int i = 0; i < positions.Count; i++) {
                Vector2Int connectionPosition = position + positions[i];
                if (m_maps.ContainsKey(connectionPosition) && !visitedMaps.Contains(connectionPosition)) {
                    connections.Add(m_maps[connectionPosition]);
                    visitedMaps.Add(connectionPosition);
                }
            }

            return connections;
        }
    }
}