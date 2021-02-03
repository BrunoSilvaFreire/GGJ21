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
                if (!matcher.IsMet(value)) {
                    final &= ~current;
                }
            }
            return final;
        }
    }
}