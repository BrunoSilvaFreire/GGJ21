using Shiroi.FX.Effects;
using UnityEngine;
namespace GGJ.Master {
    public class EffectPlayer : MonoBehaviour {
        public void PlayOnSelf(Effect effect) {
            effect.PlayIfPresent(this);
        }

        public void PlayOnPlayer(Effect effect) {
            effect.PlayIfPresent(Player.Instance);
        }
    }
}