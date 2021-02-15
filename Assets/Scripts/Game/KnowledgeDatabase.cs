using System;
using GGJ.Common;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;
namespace GGJ.Game {
    [Serializable]
    public class KnowledgeIconDictionary : SerializableDictionary<Knowledge, Sprite> { }
    [Serializable]
    public class KnowledgeDependencyDictionary : SerializableDictionary<Knowledge, KnowledgeMatcher> { }
    [Serializable]
    public class KnowledgeMatcher : Matcher<Knowledge, KnowledgeMatcher> {
        protected override bool Matches(Knowledge value, Knowledge required) {
            return (value & required) == required;
        }

    }
    [CreateAssetMenu]
    public class KnowledgeDatabase : ScriptableSingleton<KnowledgeDatabase> {
        public KnowledgeIconDictionary icons;
        public KnowledgeDependencyDictionary dependencies;
        public Knowledge GetAllKnowledge(Knowledge knowledge) {
            var found = Knowledge.None;
            if (dependencies.TryGetValue(knowledge, out var matcher)) {
                AddTo(ref found, matcher);
            }
            foreach (var individualFlag in found.IndividualFlags()) {
                if (dependencies.TryGetValue(individualFlag, out matcher)) {
                    AddTo(ref found, matcher);
                }
            }
            return found;
        }
        private void AddTo(ref Knowledge knowledge, KnowledgeMatcher matcher) {
            if (matcher.mode == KnowledgeMatcher.BitMode.Self) {
                knowledge |= matcher.data;
            } else {
                foreach (var matcherChild in matcher.children) {
                    AddTo(ref knowledge, matcherChild);
                }
            }
        }
        public Knowledge Validate(Knowledge value) {
            var final = value;
            for (var i = 0; i < sizeof(Knowledge) * 8; i++) {
                var current = (Knowledge)(1 << i);
                if (!dependencies.TryGetValue(current, out var matcher)) {
                    continue;
                }
                if (!matcher.IsMet(value)) {
                    final &= ~current;
                }
            }
            return final;
        }
    }
}