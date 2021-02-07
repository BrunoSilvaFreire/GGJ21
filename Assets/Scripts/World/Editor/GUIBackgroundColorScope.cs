using System;
using UnityEditor;
using UnityEngine;
namespace GGJ.World.Editor {
    public class GUIBackgroundColorScope : IDisposable {
        private readonly Color fallback;
        public GUIBackgroundColorScope(Color background) {
            fallback = GUI.backgroundColor;
            GUI.backgroundColor = background;
        }
        public void Dispose() {
            GUI.backgroundColor = fallback;
        }
    }
    public class HandlesColorScope : IDisposable {
        private readonly Color fallback;
        public HandlesColorScope(Color background) {
            fallback = Handles.color;
            Handles.color = background;
        }
        public void Dispose() {
            Handles.color = fallback;
        }
    }
}