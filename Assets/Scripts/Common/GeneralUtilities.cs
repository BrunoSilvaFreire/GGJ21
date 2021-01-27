using UnityEngine;
namespace Common {
    public static class GeneralUtilities {
        public static void ClearChildren(this Transform transform) {
            for (var i = 0; i < transform.childCount; i++) {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
        }

    }
}