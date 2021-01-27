using System;
using UnityEngine;
namespace Common {
    [Serializable]
    public abstract class Filter {
        public abstract bool Allowed(Collider2D collider);
    }
}