using System;
using System.Collections.Generic;
using System.Linq;
using GGJ.Common;
using GGJ.Traits.Burns;
using Sirenix.OdinInspector;
namespace GGJ.Traits.Burns {
    [Serializable]
    public class BurnFilters : BurnFilter {
        public List<BurnFilter> burns;
        public Mode mode;
        public enum Mode {
            All,
            Any,
            None
        }
        public override bool Allowed() {
            return mode switch {
                Mode.All => burns.All(node => node.Allowed()),
                Mode.Any => burns.Any(node => node.Allowed()),
                Mode.None => !burns.Any(node => node.Allowed()),
                _ => false
            };
        }
    }
    [ShowInInspector]
    public class UsedComboBefore : BurnFilter {

        public override bool Allowed() {
            return true;
        }
    }
}