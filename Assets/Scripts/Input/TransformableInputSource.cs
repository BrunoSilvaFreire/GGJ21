using System;
namespace Input {
    public delegate T Transformation<T>(T value);
    public class TransformableInputSource : InputSource {
        public InputSource delegateSource;
        public Transformation<float> horizontal;
        public Transformation<float> vertical;
        public Transformation<bool> jump;
        public Transformation<bool> interact;
        public override float GetHorizontal() {
            return Transform(delegateSource.GetHorizontal(), horizontal);
        }
        private static T Transform<T>(T value, Transformation<T> transformation) {
            if (transformation != null) {
                return transformation(value);
            }
            return value;
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
    }
}