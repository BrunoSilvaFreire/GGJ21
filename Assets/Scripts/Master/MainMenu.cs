using System;
using Input;
using UnityEngine;
using UnityEngine.Playables;
namespace GGJ.Master {
    public class MainMenu : MonoBehaviour {
        public OverridableInputSource source;
        public PlayableDirector introCutscene;
        private void Start() {
            source.overriden = OverridableInputSource.InputOverrideFlags.Horizontal | OverridableInputSource.InputOverrideFlags.Vertical | OverridableInputSource.InputOverrideFlags.Jump;
            source.horizontal = 0;
            source.vertical = 0;
            source.jump = false;
        }
        private void Update() {
            if (source.GetInteract()) {
                source.overriden = 0;
                if (introCutscene != null) {
                    introCutscene.Play();
                    Destroy(this);
                }
            }
        }
    }
}