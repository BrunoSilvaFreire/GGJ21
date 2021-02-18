using System;
namespace GGJ.Traits.Burns {
    [Serializable]
    public abstract class BurnFilter {
        public abstract bool Allowed();
    }
}