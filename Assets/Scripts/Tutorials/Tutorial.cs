using Lunari.Tsuki.Runtime;
using UnityEngine;

namespace GGJ.Tutorials {
    public class Tutorial : MonoBehaviour {
        public Animator animator;
#if UNITY_EDITOR
        private void OnValidate() {
            if (animator != null) {
                animator.EnsureHasParameter("Shown", AnimatorControllerParameterType.Bool);
            }
        }
#endif
    }
}