using System.Collections.Generic;
using System.Linq;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using Sirenix.OdinInspector;
using UnityEngine;
namespace GGJ.Traits.Burns {
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
}