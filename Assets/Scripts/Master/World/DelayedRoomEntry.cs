using GGJ.Common;
using GGJ.World.Legacy;
using Lunari.Tsuki.Entities;
using Shiroi.FX.Services.BuiltIn;
using Shiroi.FX.Utilities;
using UnityEngine;

namespace GGJ.Master.World {
    public class DelayedRoomEntry : Trait, ISetupable<Map> {
        public float duration;
        public AnimationCurve curve;
        public void Setup(Map obj) {
            obj.PlayerInside.Bind(_ => TimeController.Instance.RegisterTimedService(duration, new AnimatedTimeMeta(curve)));
        }
    }
}