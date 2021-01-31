using FMOD;
using FMOD.Studio;
using FMODUnity;
using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using UnityEngine;
namespace GGJ.Master {

    public class FMODEffect : Effect {
        [SerializeField]
        [EventRef]
        public string fmodEvent;
        private EventDescription? description;
        public override void Play(EffectContext context) {
            description ??= LoadEvent();
            var pos = context.GetRequiredFeature<PositionFeature>().Position;
            if (description.Value.createInstance(out var instance) == RESULT.OK) {
                instance.set3DAttributes(new ATTRIBUTES_3D {
                    position = pos.ToFMODVector(),
                    up = Vector3.up.ToFMODVector()
                });
                instance.start();
            }
        }
        private EventDescription LoadEvent() {
            return RuntimeManager.GetEventDescription(fmodEvent);
        }
    }
}