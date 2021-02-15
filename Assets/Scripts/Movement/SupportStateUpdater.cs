using UnityEngine;
namespace GGJ.Movement {
    public abstract class SupportStateUpdater : MonoBehaviour {
        public abstract void Test(Motor motor, ref SupportState state);
    }
}