using System;
using Common;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;
namespace GGJ.Master.UI.Knowledge {
    [Serializable]
    public class KnowledgeIconDictionary : SerializableDictionary<Knowledgeable.Knowledge, Sprite> { }
    [Serializable]
    public class KnowledgeDependencyDictionary : SerializableDictionary<Knowledgeable.Knowledge, KnowledgeMatcher> { }
    [Serializable]
    public class KnowledgeMatcher : Matcher<Knowledgeable.Knowledge, KnowledgeMatcher> {
        protected override bool Matches(Knowledgeable.Knowledge value, Knowledgeable.Knowledge required) {
            return (value & required) == required;
        }
        public Knowledgeable.Knowledge GetAllKnowledge() {
            var found = Knowledgeable.Knowledge.None;
            AddTo(ref found, this);
            return found;
        }
        private void AddTo(ref Knowledgeable.Knowledge knowledge, KnowledgeMatcher matcher) {
            if (matcher.mode == BitMode.Self) {
                knowledge |= matcher.data;
            } else {
                foreach (var matcherChild in matcher.children) {
                    AddTo(ref knowledge, matcherChild);
                }
            }
        }
    }
    [CreateAssetMenu]
    public class KnowledgeDatabase : ScriptableSingleton<KnowledgeDatabase> {
        public KnowledgeIconDictionary icons;
        public KnowledgeDependencyDictionary dependencies;
        public Knowledgeable.Knowledge Validate(Knowledgeable.Knowledge value) {
            var final = value;
            for (var i = 0; i < sizeof(Knowledgeable.Knowledge) * 8; i++) {
                var current = (Knowledgeable.Knowledge)(1 << i);
                if (!dependencies.TryGetValue(current, out var matcher)) {
                    continue;
                }
                matcher.GetAllKnowledge();
                if (!matcher.IsMet(value)) {
                    final &= ~current;
                }
            }
            return final;
        }
    }
}