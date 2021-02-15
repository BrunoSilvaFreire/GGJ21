using System.Collections.Generic;
using GGJ.Common;
using UnityEngine;
using UnityEngine.UI;
namespace GGJ.UI {
    [ExecuteInEditMode]
    public class GraphicUIGroup : MonoBehaviour {
        public BooleanHistoric overrideColor;
        public Graphic[] graphics;
        private List<Color> backup = new List<Color>();
        public Color color;

        public void EnableOverrideColor() {
            overrideColor.Current = true;
        }

        public void DisableOverrideColor() {
            overrideColor.Current = true;
        }

        private void Update() {
            overrideColor.CopyCurrentToLast();
            if (!overrideColor) {
                if (overrideColor.JustActivated) {
                    for (var i = 0; i < graphics.Length; i++) {
                        graphics[i].color = backup[i];
                    }
                }

                return;
            }

            if (overrideColor.JustActivated) {
                backup.Clear();
                foreach (var graphic in graphics) {
                    backup.Add(graphic.color);
                }
            }

            foreach (var graphic in graphics) {
                graphic.color = color;
            }
        }
    }
}