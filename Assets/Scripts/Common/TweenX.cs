using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
namespace Common {
    public static class TweenX {
        public static Tweener DOColor(this IColorized target, Color endValue, float duration) {
            endValue = endValue - target.Color;
            var to = new Color(0, 0, 0, 0);
            return DOTween.To(() => to, x => {
                var diff = x - to;
                to = x;
                target.Color += diff;
            }, endValue, duration).Blendable().SetTarget(target);
        }
    }
}