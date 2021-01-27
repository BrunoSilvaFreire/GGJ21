using Lunari.Tsuki.Runtime;
using UnityEngine;
namespace Common {
    public static class CustomDrawing {
        public static void Circle(Vector2 center, float radius) {
            Circle(center, radius, Color.white);
        }

        public static void Circle(Vector2 center, float radius, Color color) {
            Debugging.DrawWireCircle2D(center, radius, color);
        }
    }
}