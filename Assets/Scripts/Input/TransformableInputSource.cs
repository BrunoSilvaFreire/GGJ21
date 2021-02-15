namespace GGJ.Input {
    public delegate T Transformation<T>(T value);
    public class TransformableInputSource : InputSource {
        public InputSource delegateSource;
        public Transformation<float> horizontal;
        public Transformation<float> vertical;
        public Transformation<bool> jump;
        public Transformation<bool> interact;
        public Transformation<bool> cancel;
        public Transformation<bool> reset;
        public Transformation<bool> attack;
        public override float GetHorizontal() {
            return Transform(delegateSource.GetHorizontal(), horizontal);
        }
        private static T Transform<T>(T value, Transformation<T> transformation) {
            if (transformation != null) {
                return transformation(value);
            }
            return value;
        }
        public override bool GetAttack() {
            return Transform(delegateSource.GetAttack(), attack);
        }
        public override bool GetJump() {
            return Transform(delegateSource.GetJump(), jump);
        }
        public override bool GetInteract() {
            return Transform(delegateSource.GetInteract(), interact);
        }
        public override float GetVertical() {
            return Transform(delegateSource.GetVertical(), vertical);
        }
        public override bool GetCancel() {
            return Transform(delegateSource.GetCancel(), cancel);
        }
        public override bool GetReset() {
            return Transform(delegateSource.GetReset(), reset);
        }
    }
}