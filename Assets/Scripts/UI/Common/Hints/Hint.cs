using System;
using GGJ.Common;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Exceptions;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;

namespace GGJ.UI.Common.Hints {
    public class Hint : MonoBehaviour, ISetupable<string> {
        public View view;
        public SVGImage iconGraphic;
        public TMP_Text iconText;
        public TMP_Text actionText;
        public string initializeTo;
        private void Start() {
            if (!initializeTo.IsNullOrEmpty()) {
                Setup(initializeTo);
            }
        }
        public void Setup(string obj) {
            if (!HintDatabase.Instance.graphicsLookup.TryGetValue(obj, out var hint)) {
                throw new WTFException($"There is no hint for action '{obj}'");
            }
            switch (hint.hintType) {
                case HintGraphics.Type.Text:
                    iconGraphic.enabled = false;
                    iconText.enabled = true;
                    iconGraphic.sprite = hint.svgSprite;
                    break;
                case HintGraphics.Type.Graphic:
                    iconGraphic.enabled = true;
                    iconText.enabled = false;
                    iconText.text = hint.text;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            actionText.text = obj;
        }
    }
}