using System.Collections.Generic;
using System.Linq;
using Common;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GGJ.Master.UI.Knowledge {
    public class KnowledgeTerminal : MonoBehaviour {
        public KnowledgeView prefab;
        public HorizontalLayoutGroup groupPrefab;
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

            foreach (var knowledge in KnowledgeX.IndividualFlags()) {
                GetDepthOf(knowledge); // Ensure placed
            }
            var deepest = depth.Select(pair => pair.Value).Max();
            EnsureHasGroups(deepest + 1);
            foreach (var candidate in available.IndividualFlags()) {
                var currentDepth = GetDepthOf(candidate);
                var item = prefab.Clone(groups[currentDepth].transform);
                item.Setup(candidate);
                Views.Add(item);
            }
            onViewsAssigned.Invoke();
        }
        private readonly List<HorizontalLayoutGroup> groups = new List<HorizontalLayoutGroup>();
        private void EnsureHasGroups(int nGroups) {
            if (groups.Count > nGroups) {
                for (var i = nGroups; i < groups.Count; i++) {
                    Destroy(groups[i]);
                }
                groups.RemoveRange(nGroups, groups.Count - nGroups);
            }
            if (groups.Count < nGroups) {
                var toAdd = nGroups - groups.Count + 1;
                for (var i = 0; i < toAdd; i++) {
                    groups.Add(groupPrefab.Clone(transform));
                }
            }
        }
    }
}