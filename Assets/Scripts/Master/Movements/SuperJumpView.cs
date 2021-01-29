using UI;
using UnityEngine;
using UnityEngine.UI;
namespace GGJ.Master.Movements {
    public class SuperJumpView : AnimatedView {
        public Image bar;
        public Gradient color;
        public SuperJumpAttachment superJump;
        private void Update() {
            Shown = superJump.tilt.lookingDown.Current;
            bar.fillAmount = superJump.currentCharge;
            bar.color = color.Evaluate(superJump.currentCharge);
        }
    }
}