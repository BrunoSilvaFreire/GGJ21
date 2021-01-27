using UnityEngine;
namespace Movement {
    public abstract class SupportStateUpdater : MonoBehaviour {
        public abstract void Test(Motor motor, ref SupportState state);
    }
}