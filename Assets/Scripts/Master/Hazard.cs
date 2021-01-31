using GGJ.Traits.Combat;
using Lunari.Tsuki.Entities;
using UnityEngine;

namespace GGJ.Master {
    public class Hazard : MonoBehaviour {

        private void OnTriggerEnter2D(Collider2D other) {
            var e = other.GetComponentInParent<Entity>();
            if (e == null) {
                return;
            }

            if (!e.Access(out Living l)) {
                return;
            }

            if (l.canBeHurtByHazard) {
                l.Kill();
            }
        }
    }
}