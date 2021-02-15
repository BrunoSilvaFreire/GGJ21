using System;
using System.Collections;
using System.Collections.Generic;
using GGJ.Game;
using GGJ.Game.Traits;
using Lunari.Tsuki.Runtime;
using TMPro;
using UnityEngine;
namespace GGJ.UI.Common.Knowledge {
    public class KnowledgeEditorCoding : MonoBehaviour {
        private Queue<string> awaitingAddition = new Queue<string>();
        private Game.Knowledge last;
        public TMP_Text text;
        public float charactersPerSecond = 20;

        public float charCooldown {
            get => 1 / charactersPerSecond;
            set => charactersPerSecond = 1 / value;
        }

        private void Start() {
            ResetBuffer();
            var editor = PlayerUI.Instance.KnowledgeEditor;
            editor.onShow.AddListener(ResetBuffer);
            var binding = Player.Instance.Bind<Knowledgeable>();
            binding.BindToValue(knowledgeable => knowledgeable.CurrentKnowledge, OnKnowledgeChanged);
            if (binding.Current != null) {
                last = binding.Current.CurrentKnowledge;
            }
        }
        private void OnKnowledgeChanged(Game.Knowledge arg0) {
            AddLine("");
            foreach (var flag in KnowledgeX.IndividualFlags()) {
                var before = (last & flag) == flag;
                var after = (arg0 & flag) == flag;
                if (before && !after) {
                    AddLine($"removeKnowledge(Knowledge::{flag:G});");
                }
                if (!before && after) {
                    AddLine($"addKnowledge(Knowledge::{flag:G});");
                }
            }
            last = arg0;
            Flush();
        }
        private Coroutine coroutine;
        private void Flush() {
            coroutine ??= StartCoroutine(Write());
        }
        private IEnumerator Write() {
            while (!awaitingAddition.IsEmpty()) {
                var s = awaitingAddition.Dequeue();
                foreach (var ch in s) {
                    text.text += ch;
                    yield return new WaitForSeconds(charCooldown);
                }
            }
            coroutine = null;
        }

        private void ResetBuffer() {
            text.text = string.Empty;
            awaitingAddition.Clear();
            coroutine?.Stop(this);
            coroutine = null;
            AddLine("#include <apollo.h>");
            Flush();
        }

        public void AddLine(string line) {
            awaitingAddition.Enqueue($"{line}{Environment.NewLine}");
        }
    }
}