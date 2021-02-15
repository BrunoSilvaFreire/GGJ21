using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GGJ.Common;
using Lunari.Tsuki.Editor;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Exceptions;
using Lunari.Tsuki.Runtime.Scopes;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace GGJ.World.Editor {
    [Serializable]
    public class ApolloMapEditorUserData {
        public Vector2Int teleportCoordinates;
        public string query;
    }

    public class ApolloMapEditor : EditorWindow {
        private ApolloMapEditorUserData userData;
        private Vector2 scrollPos;
        public static Color SelectedColor = new Color32(0x4D, 0xD0, 0xE1, 0XFF);
        public const string EditorDataPath = "Apollo/MapEditor.json";
        [MenuItem("Apollo/Map Editor")]
        public static void Open() {
            CreateWindow<ApolloMapEditor>().ShowTab();
        }
        private void OnDisable() {
            Save();
            SceneView.duringSceneGui -= DrawBounds;
        }

        private void Save() {

            var assetsFolder = Application.dataPath;
            var path = Directory.GetParent(assetsFolder).FullName;
            var filePath = Path.Combine(path, EditorDataPath);
            var parent = Directory.GetParent(filePath);
            if (!File.Exists(parent.FullName)) {
                Directory.CreateDirectory(parent.FullName);
            }
            File.WriteAllText(filePath, JsonUtility.ToJson(userData));
        }
        private void OnEnable() {
            Load();
            titleContent = new GUIContent {
                text = "Apollo Map Editor",
                image = Icons.unityeditor_sceneview
            };
            SceneView.duringSceneGui += DrawBounds;
        }
        private void Load() {
            var assetsFolder = Application.dataPath;
            var path = Directory.GetParent(assetsFolder).FullName;
            var filePath = Path.Combine(path, EditorDataPath);
            if (File.Exists(filePath)) {
                userData = JsonUtility.FromJson<ApolloMapEditorUserData>(File.ReadAllText(filePath));
            }
            userData ??= new ApolloMapEditorUserData();
        }
        private void DrawBounds(SceneView obj) {
            var mapManager = WorldManager.Instance;
            var grid = mapManager.grid;
            if (grid == null) {
                return;
            }
            using (new HandlesColorScope(Color.magenta)) {
                foreach (var room in mapManager.GetComponentsInChildren<Room>()) {
                    Handles2.DrawWireBox2D(room.transform.position, mapManager.roomSize);
                }
            }

        }

        private void OnGUI() {
            var mapManager = WorldManager.Instance;
            if (mapManager == null) {
                EditorGUILayout.LabelField(
                    "Scene does not have a WorldManager. You can find a prefab of that in 'Content/Prefabs/World'",
                    Styles.ErrorLabel
                );
                return;
            }
            if (GUILayout.Button("Open Configuration")) {
                Selection.activeObject = MapDatabase.Instance;
            }
            using (var scope = new EditorGUILayout.ScrollViewScope(scrollPos)) {
                Teleport();
                TileMaps(mapManager);
                Palettes();
                Rooms(mapManager);
                scrollPos = scope.scrollPosition;
            }
            Repaint();
        }
        private void Palettes() {
            using (new EditorGUILayout.VerticalScope(Styles.GroupBox)) {
                EditorGUILayout.LabelField("Palettes", Styles.BoldLabel);
                using (new EditorGUILayout.HorizontalScope()) {
                    foreach (var palette in GridPaintingState.palettes) {
                        if (GUILayout.Button($"Use {palette.name}")) {
                            GridPaintingState.palette = palette;
                        }
                    }
                }
            }
        }
        private void TileMaps(WorldManager mapManager) {
            using (new EditorGUILayout.VerticalScope(Styles.GroupBox)) {
                EditorGUILayout.LabelField("Tilemaps", Styles.BoldLabel);
                using (new EditorGUILayout.HorizontalScope()) {
                    foreach (var pair in mapManager.tilemap) {
                        var key = pair.Key;
                        var value = pair.Value;
                        if (GUILayout.Button($"Edit {key}")) {
                            var database = MapDatabase.Instance;
                            GridPaintingState.scenePaintTarget = value.gameObject;

                            if (database.options.TryGetValue(key, out var options)) {
                                GridPaintingState.palette = options.palette;
                                var found = GridPaintingState.brushes.FirstOrDefault(brush => brush.GetType().Name.Equals(options.brushName));
                                if (found != null) {
                                    GridPaintingState.gridBrush = found;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void Rooms(WorldManager worldManager) {
            using (new EditorGUILayout.VerticalScope(Styles.GroupBox)) {
                EditorGUILayout.LabelField("Rooms", Styles.BoldLabel);
                var rooms = worldManager.GetComponentsInChildren<Room>();
                using (new EditorGUILayout.HorizontalScope()) {
                    var database = MapDatabase.Instance;
                    database.roomPrefab = (Room)EditorGUILayout.ObjectField("Room Prefab", database.roomPrefab, typeof(Room), false);
                    using (new GUIEnabledScope(database.roomPrefab != null)) {
                        if (GUILayout.Button("Create Room")) {
                            var prefab = (Room)PrefabUtility.InstantiatePrefab(database.roomPrefab);
                            prefab.transform.SetParent(worldManager.transform);
                            Selection.activeObject = prefab;
                        }
                        if (GUILayout.Button("Snap All")) {
                            foreach (var room in FindObjectsOfType<Room>()) {
                                Snap(worldManager, room);
                            }
                        }
                    }
                }
                userData.query = EditorGUILayout.TextField(userData.query, Styles.SearchTextField);
                var tree = new Tree<List<Room>>();
                var groups = rooms.GroupBy(room => {
                    if (worldManager.groups.TryGetValue(room, out var group)) {
                        return group;
                    }
                    return string.Empty;
                }).ToList();

                foreach (var grouping in groups) {
                    var node = tree.FindOrCreate(grouping.Key);
                    node.self ??= new List<Room>();
                    node.self.AddRange(grouping);
                }
                DrawTree(worldManager, tree, string.Empty);

            }
        }
        private Dictionary<string, bool> groupVisibility = new Dictionary<string, bool>();
        private void DrawTree(WorldManager manager, Tree<List<Room>> tree, string fullPath) {
            GUI.depth++;
            var path = $"{fullPath}{tree.name}/";
            if (!groupVisibility.TryGetValue(path, out var visible)) {
                visible = false;
            }
            visible = EditorGUILayout.Foldout(visible, $"{tree.name}/");
            groupVisibility[path] = visible;
            if (visible) {

                if (tree.self != null) {
                    foreach (var room in tree.self) {
                        if (!string.IsNullOrEmpty(userData.query) && !room.name.Contains(userData.query)) {
                            continue;
                        }
                        Room(manager, room);
                    }
                }
                var children = tree.children;
                if (!children.IsEmpty()) {
                    using (new EditorGUILayout.VerticalScope(Styles.GroupBox)) {
                        foreach (var child in children) {
                            DrawTree(manager, child, path);
                        }
                    }
                }
            }

            GUI.depth--;
        }

        private void Room(WorldManager worldManager, Room room) {
            var coords = worldManager.RoomCoordinates(room);
            var selected = Selection.activeObject;
            var isSelected = selected == room || selected == room.gameObject;
            var color = isSelected ? SelectedColor : GUI.backgroundColor;
            using (new GUIBackgroundColorScope(color)) {
                using (new GUILayout.VerticalScope(Styles.box)) {
                    room.name = EditorGUILayout.TextField("Name", room.name);
                    if (!worldManager.groups.TryGetValue(room, out var group)) {
                        group = string.Empty;
                    }
                    var before = group;
                    group = EditorGUILayout.TextField("Group", group);
                    if (!group.IsNullOrEmpty() && group != before) {
                        worldManager.groups[room] = group;
                        groupVisibility[$"/{group}/"] = true;
                    }
                    EditorGUILayout.LabelField("Coordinates", coords.ToString());
                    using (new EditorGUILayout.HorizontalScope()) {
                        if (GUILayout.Button("Select")) {
                            Selection.activeObject = room;
                        }
                        if (GUILayout.Button("Go To")) {
                            userData.teleportCoordinates = coords;
                            Selection.activeObject = room;
                            GoTo();
                        }

                        if (GUILayout.Button("Snap")) {
                            Snap(worldManager, room);
                        }
                        if (GUILayout.Button(Icons.treeeditor_trash, GUILayout.Height(22), GUILayout.Width(22))) {
                            DestroyImmediate(room.gameObject);
                        }
                    }
                }
            }
        }
        private static void Snap(WorldManager worldManager, Room room) {
            var coords = worldManager.RoomCoordinates(room);
            var newPos = worldManager.LocalToWorld(coords);
            room.transform.position = newPos;
        }
        private void Teleport() {
            using (new EditorGUILayout.VerticalScope(Styles.GroupBox)) {
                EditorGUILayout.LabelField("Go To Room", Styles.BoldLabel);
                using (new EditorGUILayout.HorizontalScope()) {
                    userData.teleportCoordinates = EditorGUILayout.Vector2IntField("Coordinates", userData.teleportCoordinates);
                    var content = new GUIContent {
                        text = "Go to",
                        image = Icons.processed_unityengine_animations_positionconstraint_icon_asset
                    };
                    var height = EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector2Int, content);
                    if (GUILayout.Button(content, GUILayout.Height(height), GUILayout.MaxWidth(24 * 4))) {
                        GoTo();
                    }
                }
            }
        }
        private void GoTo() {
            foreach (SceneView window in SceneView.sceneViews) {

                var pos = window.pivot;
                var world = WorldManager.Instance;

                var tgt = world.LocalToWorld(userData.teleportCoordinates);
                pos.x = tgt.x;
                pos.y = tgt.y;
                window.pivot = pos;
            }
        }
    }

}