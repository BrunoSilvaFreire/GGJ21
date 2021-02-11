using System;
using Common;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;
namespace GGJ.Master.UI.Knowledge {
    [Serializable]
    public class KnowledgeIconDictionary : SerializableDictionary<Traits.Knowledge.Knowledge, Sprite> { }
    [Serializable]
    public class KnowledgeDependencyDictionary : SerializableDictionary<Traits.Knowledge.Knowledge, KnowledgeMatcher> { }
    [Serializable]
    public class KnowledgeMatcher : Matcher<Traits.Knowledge.Knowledge, KnowledgeMatcher> {
        protected override bool Matches(Traits.Knowledge.Knowledge value, Traits.Knowledge.Knowledge required) {
            return (value & required) == required;
        }

    }
    [CreateAssetMenu]
    public class KnowledgeDatabase : ScriptableSingleton<KnowledgeDatabase> {
        public KnowledgeIconDictionary icons;
        public KnowledgeDependencyDictionary dependencies;
        public Traits.Knowledge.Knowledge GetAllKnowledge(Traits.Knowledge.Knowledge knowledge) {
            var found = Traits.Knowledge.Knowledge.None;
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
        private void AddTo(ref Traits.Knowledge.Knowledge knowledge, KnowledgeMatcher matcher) {
            if (matcher.mode == KnowledgeMatcher.BitMode.Self) {
                knowledge |= matcher.data;
            } else {
                foreach (var matcherChild in matcher.children) {
                    AddTo(ref knowledge, matcherChild);
                }
            }
        }
        public Traits.Knowledge.Knowledge Validate(Traits.Knowledge.Knowledge value) {
            var final = value;
            for (var i = 0; i < sizeof(Traits.Knowledge.Knowledge) * 8; i++) {
                var current = (Traits.Knowledge.Knowledge)(1 << i);
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