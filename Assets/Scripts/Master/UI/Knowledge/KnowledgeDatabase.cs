using System;
using GGJ.Traits.Knowledge;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;
namespace GGJ.Master.UI.Knowledge {
    [Serializable]
    public class KnowledgeIconDictionary : SerializableDictionary<Knowledgeable.Knowledge, Sprite> { }
    [CreateAssetMenu]
    public class KnowledgeDatabase : ScriptableSingleton<KnowledgeDatabase> {
        public KnowledgeIconDictionary icons;
    }
}