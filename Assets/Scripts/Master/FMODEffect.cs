using FMOD;
using FMOD.Studio;
using FMODUnity;
using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GGJ.Master {
    public class FMODEffect : Effect {
        [SerializeField]
        [EventRef]
        public string fmodEvent;
        public float volume;
        private EventDescription description;
        public override void Play(EffectContext context) {
            if (!description.isValid()) {
                description = LoadEvent();
            }
            var pos = context.GetRequiredFeature<PositionFeature>().Position;
            RESULT result;
            if ((result = description.createInstance(out var instance)) == RESULT.OK) {
                instance.set3DAttributes(new ATTRIBUTES_3D {
                    position = pos.ToFMODVector(),
                    up = Vector3.up.ToFMODVector()
                });
                instance.setVolume(volume);
                instance.start();
            } else {
                Debug.Log($"Fmod Error: {result}");
            }
        }
        private EventDescription LoadEvent() {
            return RuntimeManager.GetEventDescription(fmodEvent);
        }
    }
}