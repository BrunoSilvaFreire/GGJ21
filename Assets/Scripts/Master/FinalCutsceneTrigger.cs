using FMODUnity;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine.Playables;
namespace GGJ.Master {
    public class FinalCutsceneTrigger : Singleton<FinalCutsceneTrigger> {
        public PlayableDirector director;
        public StudioEventEmitter normalBGM, happyBGM, sadBGM;
        public void Play() {
            director.Play();
            normalBGM.Stop();
            happyBGM.Play();
        }
    }
}