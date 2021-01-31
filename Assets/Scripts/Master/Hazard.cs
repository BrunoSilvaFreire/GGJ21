using System;
using System.Collections.Generic;
using Common;
using GGJ.Traits.Combat;
using Lunari.Tsuki.Entities;
using Shiroi.FX.Effects;
using Sirenix.OdinInspector;
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

            l.Kill();
        }
    }
}