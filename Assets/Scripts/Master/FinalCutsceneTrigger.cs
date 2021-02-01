using FMODUnity;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;
using UnityEngine.Playables;
namespace GGJ.Master {
    public class FinalCutsceneTrigger : Singleton<FinalCutsceneTrigger> {
        public PlayableDirector director;
        public StudioEventEmitter normalBGM, happyBGM, sadBGM;
        private bool played;
        public Animator athenaAnimator;
        public void Play() {
            if (!played) {
                athenaAnimator.enabled = true;
                director.Play();
                normalBGM.Stop();
                happyBGM.Play();
                played = true;
            }
        }
    }
}