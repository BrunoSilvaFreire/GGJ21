using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR

#endif

namespace GGJ.Common.Deployables {
    public class Deployable : MonoBehaviour {
        public static Deployable Create(string name) {
            var obj = new GameObject($"Deployable: {name}") {
                hideFlags = HideFlags.DontSave | HideFlags.NotEditable
            };
            return obj.AddComponent<Deployable>();
        }

        public void Delete() {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                DestroyImmediate(gameObject);
                return;
            }
#endif
            Destroy(gameObject);
        }

        public void ProximityTrigger(Vector2 position, float radius, UnityAction onEntered, Filters filters = null) {
            var trigger = gameObject.AddComponent<Trigger>();
            var col = gameObject.AddComponent<CircleCollider2D>();
            transform.position = position;
            col.radius = radius;
            col.isTrigger = true;
            trigger.filters = filters;
            var e = trigger.onTriggered ?? (trigger.onTriggered = new UnityEvent());
            e.AddListener(onEntered);
        }
    }
}