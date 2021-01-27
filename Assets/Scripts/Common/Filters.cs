using System;
using System.Collections.Generic;
using System.Linq;
using Lunari.Tsuki.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Common {
    [Serializable]
    public class Filters : Filter {
        [SerializeReference, ShowInInspector]
        public List<Filter> filters;

        public override bool Allowed(Collider2D collider) {
            if (filters == null || filters.IsEmpty()) {
                return true;
            }

            return filters.All(filter => filter.Allowed(collider));
        }
    }
}