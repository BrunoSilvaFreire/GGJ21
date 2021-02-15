using System;
using System.Collections;
using GGJ.Game;
using GGJ.Game.Traits;
using GGJ.Persistence;
using GGJ.Traits.Combat;
using GGJ.UI.Common.Knowledge;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;
using UnityEngine.UI;
namespace GGJ.UI.Common {
    [ExecuteInEditMode]
    public class PlayerUI : Singleton<PlayerUI> {
        public KnowledgeEditor knowledgeEditor;
        public View deathCurtains;
        public RenderTexture gameTexture;
        public RawImage uiImage;
        public AspectRatioFitter uiRatio;
        public float timeUntilLoad = .5F;
        public float minWaitTime = .5F;
        private float startTime;
        public RenderTexture UITexture => (RenderTexture)uiImage.texture;
        private void Start() {
            var manager = PersistenceManager.Instance;
            manager.BeforeRestart(BeforeRestart);
            manager.AfterRestart(AfterRestart);
        }
        private IEnumerator AfterRestart() {
            Time.timeScale = 1;
            var remaining = minWaitTime - (Time.time - startTime);
            if (remaining > 0) {
                yield return new WaitForSecondsRealtime(remaining);
            }
            var p = Player.Instance.Pawn;
            if (p != null) {
                if (p.Access(out Living living)) {
                    living.Alive = true;
                }
                if (p.Access(out Filmed filmed)) {
                    filmed.MoveToEntity();
                } else {
                    Player.Instance.camera.transform.position = p.transform.position;
                }
            }
            deathCurtains.Hide();
        }
        private IEnumerator BeforeRestart() {
            startTime = Time.time;
            deathCurtains.Show();
            yield return new WaitForSecondsRealtime(timeUntilLoad);
        }
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