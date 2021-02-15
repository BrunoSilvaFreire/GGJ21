using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace GGJ.World {
    public static class TsukiExtensions {
        public static List<T> GetSubObjectOfType<T>(this ScriptableObject asset) where T : class {
#if UNITY_EDITOR
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(asset));

            List<T> ofType = new List<T>();

            foreach (Object o in objs) {
                if (o is T oT) {
                    ofType.Add(oT);
                }
            }

            return ofType;
#endif
            return null;
        }

        public static Object AddObjectToAsset<T>(this ScriptableObject obj, T asset, bool setDirty = true, bool save = true) where T : Object {
#if UNITY_EDITOR
            AssetDatabase.AddObjectToAsset(asset, obj);
            if (setDirty) {
                EditorUtility.SetDirty(obj);
            }

            if (save) {
                AssetDatabase.SaveAssets();
            }

            return asset;
#endif
            return null;
        }
    }
}