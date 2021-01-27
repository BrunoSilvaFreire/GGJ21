using Common;
using UnityEngine;
namespace Props.Activatables {
    public class Activator : MonoBehaviour {
        public Activatable activatable;

        [SerializeReference]
        public Filter filter;

        private void OnTriggerEnter2D(Collider2D other) {
            var f = filter;
            if (f != null && !f.Allowed(other)) {
                return;
            }

            activatable.on = !activatable.on;
        }
    }
}