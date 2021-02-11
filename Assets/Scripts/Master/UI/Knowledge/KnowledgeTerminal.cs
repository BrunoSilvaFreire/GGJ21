using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.Extensions;

namespace GGJ.Master.UI.Knowledge {
    public class KnowledgeTerminal : MonoBehaviour {
        public KnowledgeView prefab;
        public View view;
        public GameObject container;
        public UnityEvent onViewsAssigned;
        public CanvasGroup group;
        public float slotSize = 128;

        public List<KnowledgeView> Views {
            get;
            private set;
        }

        private void Start() {
            GameManager.Instance.onAvailableKnowledgeFound.AddListener(OnAvailableChanged);
            OnAvailableChanged();
            Player.Instance.Bind<Knowledgeable>().BindToValue(
                knowledgeable => knowledgeable.CurrentKnowledge,
                delegate {
                    if (view.Shown) {
                        foreach (var view in Views) {
                            TryUpdateVisibility(view, true);
                        }
                    }
                }
            );
            view.onShow.AddListener(delegate {
                foreach (var view in Views) {
                    TryUpdateVisibility(view, true);
                }
            });
            view.onHide.AddListener(delegate {
                foreach (var view in Views) {
                    TryUpdateVisibility(view, false);
                }
            });
        }
        private void TryUpdateVisibility(KnowledgeView view, bool visible) {
            var p = Player.Instance.Pawn;
            if (p != null && p.Access(out Knowledgeable knowledgeable)) {
                var db = KnowledgeDatabase.Instance;
                if (db.dependencies.TryGetValue(view.Knowledge, out var matcher)) {
                    var allowed = matcher.IsMet(knowledgeable.CurrentKnowledge);
                    if (!allowed) {
                        visible = false;
                    }
                    view.button.interactable = allowed;
                }
            }
            view.Shown = visible;
        }
        private void OnAvailableChanged() {
            var available = GameManager.Instance.AvailableKnowledge;
            container.transform.ClearChildren();
            Views = new List<KnowledgeView>();
            var depth = new Dictionary<Traits.Knowledge.Knowledge, int>();
            var database = KnowledgeDatabase.Instance;
            var dependencies = database.dependencies;

            int GetDepthOf(Traits.Knowledge.Knowledge knowledge) {

                if (depth.TryGetValue(knowledge, out var existing)) {
                    return existing;
                }
                var result = 0;
                if (dependencies.ContainsKey(knowledge)) {
                    var toCheck = database.GetAllKnowledge(knowledge);
                    result = toCheck.IndividualFlags().Select(GetDepthOf).Max() + 1;
                }
                depth[knowledge] = result;
                return result;
            }

            var width = new Dictionary<int, int>();
            foreach (var knowledge in KnowledgeX.IndividualFlags()) {
                GetDepthOf(knowledge); // Ensure placed
            }
            foreach (var candidate in available.IndividualFlags()) {
                var item = prefab.Clone(container.transform);
                item.Setup(candidate);
                Views.Add(item);
                var viewDepth = GetDepthOf(candidate);
                if (!width.TryGetValue(viewDepth, out var viewWidth)) {
                    viewWidth = 0;
                }
                var rect = ((RectTransform)item.transform);
                rect.anchoredPosition = new Vector3(viewWidth * slotSize, -viewDepth * slotSize);
                rect.anchorMin = Vector2.up;
                rect.anchorMax = Vector2.up;
                width[viewDepth] = viewWidth + 1;
            }
            onViewsAssigned.Invoke();
        }
    }
}