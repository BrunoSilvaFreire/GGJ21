using UnityEngine;
namespace GGJ.Common {
    public static class Vectors2 {
        public static Vector2 Center(Vector2 a, Vector2 b) {
            return a + ((b - a) / 2);
        }
    }
}