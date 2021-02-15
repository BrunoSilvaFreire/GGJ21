using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.Events;
namespace GGJ.Movement {
    [Serializable]
    public class MotorEvent : UnityEvent<Motor> {
    }

    public static class Motors {
        public static Vector2 BasePosition(this Motor motor) {
            var bounds = motor.collider.bounds;
            var pos = (Vector2) bounds.center;
            pos.y -= bounds.size.y / 2;
            return pos;
        }

        public static Vector2 GetEntityPosition(this Entity entity) {
            if (entity.Access(out Motor motor)) {
                return motor.BasePosition();
            }

            return entity.transform.position;
        }

        public static int DirectionTo(this Entity self, Entity target) {
            var from = self.GetEntityPosition();
            var to = target.GetEntityPosition();
            return Math.Sign(from.x - to.x);
        }

        public static float DistanceTo(this Entity self, Entity target) {
            var from = self.GetEntityPosition();
            var to = target.GetEntityPosition();
            return Vector2.Distance(from, to);
        }

        public static TweenerCore<float, float, FloatOptions> DOControl(this Motor motor, float target, float duration) {
            return DOTween.To(
                () => motor.Control,
                value => motor.Control = value,
                target,
                duration
            ).SetTarget(motor);
        }
        public static TweenerCore<float, float, FloatOptions> DoLeftControl(this Motor motor, float target, float duration) {
            return DOTween.To(
                () => motor.LeftControl,
                value => motor.LeftControl = value,
                target,
                duration
            ).SetTarget(motor);
        }
        public static TweenerCore<float, float, FloatOptions> DoRightControl(this Motor motor, float target, float duration) {
            return DOTween.To(
                () => motor.RightControl,
                value => motor.RightControl = value,
                target,
                duration
            ).SetTarget(motor);
        }
    }
}