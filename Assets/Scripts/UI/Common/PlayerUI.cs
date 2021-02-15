using System;
using GGJ.UI.Common.Knowledge;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;
using UnityEngine.UI;
namespace GGJ.UI.Common {
    [ExecuteInEditMode]
    public class PlayerUI : Singleton<PlayerUI> {
        public KnowledgeEditor KnowledgeEditor;
        public View deathCurtains;
        public RenderTexture gameTexture;
        public RawImage uiImage;
        public AspectRatioFitter uiRatio;

        public RenderTexture UITexture => (RenderTexture)uiImage.texture;
        private void Update() {
            CheckUITexture();
        }
        private void CheckUITexture() {
            var tex = UITexture;
            var aspect = (float)gameTexture.width / gameTexture.height;
            var height = Mathf.RoundToInt(uiImage.rectTransform.rect.height);
            var targetWidth = Mathf.RoundToInt(height * aspect);
            uiRatio.aspectRatio = aspect;
            if (tex.width != targetWidth) {
                tex.Release();
                tex.height = height;
                tex.width = (int)(height * aspect);
                tex.Create();
            }
        }
    }
}