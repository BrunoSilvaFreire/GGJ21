using System;
using UnityEngine;
namespace GGJ.Common {
    [Serializable]
    public abstract class Filter {
        public abstract bool Allowed(Collider2D collider);
    }
}