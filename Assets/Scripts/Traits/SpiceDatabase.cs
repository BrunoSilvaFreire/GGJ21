using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using Sirenix.OdinInspector;
using UnityEngine;
namespace GGJ.Traits {
    [CreateAssetMenu]
    public class SpiceDatabase : ScriptableSingleton<SpiceDatabase> {
        [TableList]
        public List<Burn> respawnBurns;
        [TableList]
        public List<Burn> proximityBurns;
        [TableList]
        public List<Burn> knowledgeChangedBurns;
        public Burn SelectRespawn() => Select(respawnBurns);
        public Burn SelectKnowledge() => Select(knowledgeChangedBurns);
        public Burn SelectProximity() => Select(proximityBurns);
        private static Burn Select(List<Burn> burns) {
            var available = burns.Where(burn => burn.filter == null || burn.filter.Allowed());
            var top = available.GroupBy(burn => burn.score).MaxBy(grouping => grouping.Key);
            return top.ToList().RandomElement();
        }
        
    }
    [Serializable]
    public abstract class BurnFilter {
        public abstract bool Allowed();
    }
    [Serializable]
    public struct Burn {
        [TextArea]
        [TableColumnWidth(128)]
        public string message;
        [TableColumnWidth(128, resizable: false)]
        public string category;
        [TableColumnWidth(64, resizable: false)]
        public uint score;
        [SerializeReference]
        [TableColumnWidth(384)]
        public BurnFilter filter;
    }
}