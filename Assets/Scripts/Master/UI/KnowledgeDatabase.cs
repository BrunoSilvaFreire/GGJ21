using System;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;
namespace GGJ.Master.UI {
    [Serializable]
    public class KnowledgeIconDictionary : SerializableDictionary<Knowledgeable.Knowledge, Sprite> { }
    [CreateAssetMenu]
    public class KnowledgeDatabase : ScriptableSingleton<KnowledgeDatabase> {
        public KnowledgeIconDictionary icons;
    }
}