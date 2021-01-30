using System;
using System.Collections.Generic;
using Lunari.Tsuki.Graphs;
using Lunari.Tsuki.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace World {
    
    [Serializable]
    public struct MapVertex {
        public string scene;
        public Vector2Int position;
    }

    [Serializable]
    public class Vector2IntMapVertexLookup : SerializableDictionary<Vector2Int, MapVertex> {}


    [CreateAssetMenu(fileName = "MapConnectionConfig", menuName = "GGJ/World/MapGraph", order = 0)]
    public class MapConnectionConfig : Lunari.Tsuki.Runtime.Singletons.ScriptableSingleton<MapConnectionConfig> {
        
        public Vector2IntMapVertexLookup vector2Int2MapVertex;

        public void AddMap(Scene scene, int x, int y) {
            AddMap(scene, new Vector2Int(x, y));
        }
        public void AddMap(Scene scene, Vector2Int position) {
            if (vector2Int2MapVertex.ContainsKey(position)) {
                Debug.LogFormat("Key already present in map: {0}", position);
                var vertex = vector2Int2MapVertex[position];
                if (scene.name.Equals(vertex.scene) && vertex.position == position) {
                    Debug.Log("Same scene and position, ignoring insertion attempt");
                    return;    
                }
                Debug.LogErrorFormat("Overriding existing value for key; Old scene: {0}; New scene {1};\nOld position {2}, New position {3}",
                    vertex.scene,
                    scene.name,
                    vertex.position,
                    position);
            }
            vector2Int2MapVertex.Add(position, new MapVertex{scene = scene.name, position = position});
            EditorUtility.SetDirty(this);
        }
        
        public List<MapVertex> GetConnections(int x, int y) {
            return GetConnections(new Vector2Int(x, y));
        }

        public List<MapVertex> GetConnections(Vector2Int position) {

            if (!vector2Int2MapVertex.ContainsKey(position)) {
                Debug.LogWarningFormat("Invalid position for GetConnections: {0}", position);
                return new List<MapVertex>();
            }
            
            var connections = new List<MapVertex>();

            for (int x = -1; x < 2; x++) {
                for (int y = -1; y < 2; y++) {
                    if (x == 0 && y == 0) {
                        continue;
                    }
                    Vector2Int connectionPosition = position + new Vector2Int(x, y);
                    if (vector2Int2MapVertex.ContainsKey(connectionPosition)) {
                        connections.Add(vector2Int2MapVertex[connectionPosition]);
                    }
                }
            }
            return connections;
        }
    }
}