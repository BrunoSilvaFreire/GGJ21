using System.Collections;
using UnityEngine;
namespace UI {
    public static class Views {
        public static void SetShownForDuration(this View view, bool shown, float duration) {
            view.StartCoroutine(ShownForDuration(view, shown, duration));
        }
        public static IEnumerator SetShownInSeconds(this View view, bool shown, float duration) {
            yield return new WaitForSeconds(duration);
            view.Shown = shown;
        }
        private static IEnumerator ShownForDuration(View view, bool shown, float duration) {
            var modifier = view.AddShownModifier(shown);
            yield return new WaitForSeconds(duration);
            view.RemoveShownModifier(modifier);
        }
    }
}