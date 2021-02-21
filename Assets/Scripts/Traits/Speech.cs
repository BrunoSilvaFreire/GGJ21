using Febucci.UI;
using Lunari.Tsuki.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
namespace GGJ.Traits {
    public class Speech : Trait {
        [MinMaxSlider(0, 3)]
        public Vector2 pitch;
        public AudioSource source;
        public TextAnimatorPlayer animator;
        private void Start() {
            animator.onCharacterVisible.AddListener(OnPlayed);
        }
        private void OnPlayed(char arg0) {
            source.Stop();
            source.pitch = pitch.x + Random.value * (pitch.y - pitch.x);
            source.Play();
        }
    }
}