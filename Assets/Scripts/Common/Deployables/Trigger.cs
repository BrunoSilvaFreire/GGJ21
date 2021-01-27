using UnityEngine;
using UnityEngine.Events;
namespace Common.Deployables {
    public class Trigger : MonoBehaviour {
        public Filters filters;

        public UnityEvent onTriggered;

        private void OnTriggerEnter2D(Collider2D other) {
            if (filters != null && !filters.Allowed(other)) {
                return;
            }
            onTriggered.Invoke();
        }
    }
}