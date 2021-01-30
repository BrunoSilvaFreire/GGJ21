using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Lunari.Tsuki.Editor;
using Lunari.Tsuki.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using World;
using Debug = UnityEngine.Debug;

namespace GGJ.World.Editor {
    
    public class WorldGenerator : EditorWindow {
        
        [MenuItem("Tools/GGJ/World generator")]
        public static void Preview() {
            CreateWindow<WorldGenerator>().Show();
        }
        
        private WorldGeneratorConfig m_config;
        private string m_folderPath;

        private void OnGUI() {

            m_config = EditorGUILayout.ObjectField(m_config, typeof(WorldGeneratorConfig), false) as WorldGeneratorConfig;
            
            if (GUILayout.Button("Search json folder")) {
                m_folderPath = EditorUtility.OpenFolderPanel("Json folder", Application.dataPath, "");
            }

            m_folderPath = EditorGUILayout.TextField(m_folderPath);
            
            if (!m_folderPath.IsNullOrEmpty()) {
                if (GUILayout.Button("Load")) {
                    if (m_config == null) {
                        Debug.LogError("Please provide a config asset.");
                        return;
                    }
                    
                    var files = Directory.GetFiles(m_folderPath);
                    var jsons = files.Where(file => file.EndsWith(".json"));
                    GameObject go = null;

                    try {
                        go = PrefabUtility.InstantiatePrefab(m_config.worldPrefab) as GameObject;

                        foreach (var json in jsons) {
                            var data = File.ReadAllText(json);
                            var map = JsonUtility.FromJson<MapData>(data);
                            ProcessMap(map, go.transform);
                        }
                        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                    }
                    catch (Exception e) {
                        Debug.LogException(e);
                        if (go != null) {
                            DestroyImmediate(go);
                        }
                    }
                }    
            }
        }

        private void ProcessMap(MapData map, Transform parent) {
            
            try {
                ProcessTileSet(map.tilesets[0]);//only process first

                GameObject go = PrefabUtility.InstantiatePrefab(m_config.string2MapConfig[map.type].prefab, parent) as GameObject;
                
                foreach (var layer in map.layers) {
                    ProcessLayer(layer, go.transform);
                }

                go.GetComponent<ITiledMap>()?.Setup(map);
            }
            catch (Exception e) {
                Vector2Int mapPosition = Vector2Int.zero;
                foreach (var property in map.properties) {
                    switch (property.name) {
                        case "mapx":
                            mapPosition += new Vector2Int(int.Parse(property.value), 0);
                            break;
                        case "mapy":
                            mapPosition += new Vector2Int(0, int.Parse(property.value));
                            break;
                    }
                }
                Debug.LogErrorFormat("Map error: position:{0}", mapPosition);
                throw;
            }
        }

        private void ProcessTileSet(TileSetData tileset) {
            try {
                ClearAllTiles();
                var texture = m_config.string2TileSetConfig[tileset.name].texture;

                int rowCount = tileset.tilecount / tileset.columns;
                
                for (uint i = 0; i < tileset.tilecount; i++) {
                    int tileX = (int) i % tileset.columns;
                    int tileY =  rowCount - (int)(i / tileset.columns);
                    Rect rect = new Rect {
                        min = new Vector2(tileX * tileset.tilewidth, (tileY - 1) * tileset.tileheight),
                        max = new Vector2((tileX + 1) * tileset.tilewidth, tileY * tileset.tileheight)
                    };
                    Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 16);
                    sprite.name = string.Format("tileset_{0}_sprite_{1}", tileset.name, i);
                    var tileAsset = m_config.AddToAssetFile<Tile>(true, false);
                    tileAsset.name = string.Format("tileset_{0}_tile_{1}", tileset.name, i);
                    tileAsset.sprite = sprite;
                    tileAsset.AddObjectToAsset(sprite, true, false);
                    var tileConfig = new TileConfig {tile = tileAsset};
                    m_config.uint2TileConfig.Add(i, tileConfig);
                }
            }
            catch (Exception e) {
                Debug.LogErrorFormat("Tileset error: name: {0}", tileset.name);
                throw;
            }
            EditorUtility.SetDirty(m_config);
            AssetDatabase.SaveAssets();
        }

        public void ClearAllTiles() {
            m_config.uint2TileConfig.Clear();
             var tiles = m_config.GetSubObjectOfType<Tile>();
             for (int i = 0; i < tiles.Count; i++) {
                 AssetDatabase.RemoveObjectFromAsset(tiles[i]);
             }
             var sprites = m_config.GetSubObjectOfType<Sprite>();
             for (int i = 0; i < sprites.Count; i++) {
                 AssetDatabase.RemoveObjectFromAsset(sprites[i]);
             }
             EditorUtility.SetDirty(m_config);
             AssetDatabase.SaveAssets();

        }

        private void ProcessLayer(LayerData layer, Transform parent) {
            try {
                var go = PrefabUtility.InstantiatePrefab(m_config.string2LayerConfig[layer.name].prefab, parent) as GameObject;
                go.name = layer.name;
                if (layer.objects != null && layer.objects.Length > 0) {
                    foreach (var objectData in layer.objects) {
                        ProcessObject(objectData, go.transform, layer.height);
                    }
                }
                else if (layer.data != null && layer.data.Length > 0) {
                    go.transform.position += new Vector3(0, -layer.height);
                    var tilemap = go.GetComponent<Tilemap>();
                    for (var i = 0; i < layer.data.Length; i++) {
                        ProcessTile(layer.data[i], i % layer.width, layer.height - (i / layer.width), tilemap);
                    }
                }

                go.GetComponent<ITiledLayer>()?.Setup(layer);
            }
            catch (Exception e) {
                Debug.LogErrorFormat("Layer error: name: {0}, type: {1}", layer.name, layer.type);
                throw;
            }
        }

        private void ProcessObject(ObjectData obj, Transform parent, int layerHeight) {
            try {

                var go = PrefabUtility.InstantiatePrefab(m_config.string2ObjectConfig[obj.type].prefab, parent) as GameObject;
                go.name = obj.name;
                go.transform.position = (new Vector3(obj.x, layerHeight - obj.y) / 16) + (new Vector3(obj.width, 0) / 32) + new Vector3(0, 1);
                go.GetComponent<ITiledObject>()?.Setup(obj);
            }
            catch (Exception e) {
                Debug.LogErrorFormat("Object error: id: {0}, name: {1}, type: {2}", obj.id, obj.name, obj.type);
                throw;
            }
        }

        private void ProcessTile(uint tile, int x, int y, Tilemap tilemap) {
            if (tile == 0) {
                return;
            }
            try {
                var tileConfig = m_config.uint2TileConfig[tile - 1];
                tilemap.SetTile(new Vector3Int(x, y, 0), tileConfig.tile);
            }
            catch (Exception e) {
                Debug.LogErrorFormat("Error: tile: {0}", tile);
                throw;
            }
        }
    }
}