using System;
using System.Collections.Generic;
using Lunari.Tsuki.Graphs;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace World {
    
    public class MapManager : Singleton<MapManager> {

        private Dictionary<string, AsyncOperation> m_loadedScenes = new Dictionary<string, AsyncOperation>();

        [SerializeField] private string m_firstScene;

        private void Awake() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            m_loadedScenes[m_firstScene] = SceneManager.LoadSceneAsync(m_firstScene, LoadSceneMode.Additive);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            Debug.Log("carreguei porra");
        }
        

        public void SetActiveMap(Map map) {
            var connections = MapConnectionConfig.Instance.GetConnections(map.Coordinates);
            LoadConnections(connections);
        }

        private void LoadConnections(List<MapVertex> connections) {
            foreach (var connection in connections) {
                if (!m_loadedScenes.ContainsKey(connection.scene)) {
                    var async = SceneManager.LoadSceneAsync(connection.scene, LoadSceneMode.Additive);
                    async.allowSceneActivation = true;
                    m_loadedScenes[connection.scene] = async;
                }
            }
        }
    }
}