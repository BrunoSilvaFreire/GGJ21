using GGJ.Common;
using GGJ.Game;
using Lunari.Tsuki.Runtime;
using Shiroi.FX.Effects;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ.UI.Common.Knowledge {
    public class KnowledgeView : AnimatedView, ISetupable<Game.Knowledge> {
        public Image svg;
        public Button button;
        private Game.Knowledge knowledge;
        private const string AssignedName = "Assigned";
        private static readonly int Assigned = Animator.StringToHash(AssignedName);

        private const string DependencyName = "Dependency";
        private static readonly int Dependency = Animator.StringToHash(DependencyName);
        public Effect onPressed;
        private const string RemovedName = "Removed";
        private static readonly int Removed = Animator.StringToHash(RemovedName);
        public int resetLayer = 1;
        public Game.Knowledge Knowledge => knowledge;
        private void Start() {
            if (button) {
                button.onClick.AddListener(delegate {
                    onPressed.PlayIfPresent(this);
                });
            }
        }
        public void SetShownAsDependency(bool shownAsDependency) {
            if (shownAsDependency && animator.GetBool(Dependency)) {
                //Reset
                var state = animator.GetCurrentAnimatorStateInfo(resetLayer);
                animator.Play(state.fullPathHash, resetLayer, 0);
            }
            animator.SetBool(Dependency, shownAsDependency);
        }
#if UNITY_EDITOR
        protected override void OnValidate() {
            animator.EnsureHasParameter(AssignedName, AnimatorControllerParameterType.Trigger);
            animator.EnsureHasParameter(RemovedName, AnimatorControllerParameterType.Trigger);
            animator.EnsureHasParameter(DependencyName, AnimatorControllerParameterType.Bool);
        }
#endif

        public void Setup(Game.Knowledge obj) {
            if (obj == Game.Knowledge.None) {
                svg.enabled = false;
            }
            if (obj == knowledge) {
                return;
            }
            name = $"KnowledgeView - {obj}";
            knowledge = obj;
            var success = KnowledgeDatabase.Instance.icons.TryGetValue(obj, out var sprite);
            if (success) {
                svg.sprite = sprite;
            }

            animator.SetTrigger(obj == Game.Knowledge.None ? Removed : Assigned);
            svg.enabled = success;
        }
    }
}