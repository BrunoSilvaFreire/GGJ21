using System;
using GGJ.Common;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using Sirenix.OdinInspector;
using UnityEngine;
namespace GGJ.UI.Common.Hints {
    [CreateAssetMenu]
    public class HintDatabase : ScriptableSingleton<HintDatabase> {
        public HintGraphicsLookup graphicsLookup;
    }
    [Serializable]
    public class HintGraphicsLookup : SerializableDictionary<string, HintGraphics> { }
    [Serializable]
    public struct HintGraphics {
        public Type hintType;
        public GamePlatform platform;
        public enum Type {
            Text,
            Graphic
        }

        public bool IsText => hintType == Type.Text;
        public bool IsGraphic => hintType == Type.Graphic;
        [ShowIf(nameof(IsText))]
        public string text;
        [ShowIf(nameof(IsGraphic))]
        public Sprite svgSprite;
    }
}