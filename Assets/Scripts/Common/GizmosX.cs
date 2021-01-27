using System;
using UnityEngine;
namespace Common {
    public class GizmosColorScope : IDisposable{
        private Color fallback;

        public GizmosColorScope(Color newColor) {
            fallback = Gizmos.color;
            Gizmos.color = newColor;
        }

        public void Dispose() {
            Gizmos.color = fallback;
        }
    }
    public class GizmosX {
        public static void ForGizmo(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f) {
            ForGizmo(pos, direction, Gizmos.color, arrowHeadLength, arrowHeadAngle, arrowPosition);
        }

        public static void ForGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f) {
            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);
            DrawArrowEnd(pos, direction, color, arrowHeadLength, arrowHeadAngle, arrowPosition);
        }

        private static void DrawArrowEnd(Vector3 pos, Vector3 direction, Color color,
            float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f) {
            var right =
                (Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back) *
                arrowHeadLength;
            var left =
                (Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back) *
                arrowHeadLength;
            var up = (Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back) *
                     arrowHeadLength;
            var down =
                (Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back) *
                arrowHeadLength;

            var arrowTip = pos + (direction * arrowPosition);

            Gizmos.color = color;
            Gizmos.DrawRay(arrowTip, right);
            Gizmos.DrawRay(arrowTip, left);
            Gizmos.DrawRay(arrowTip, up);
            Gizmos.DrawRay(arrowTip, down);
        }
    }
}