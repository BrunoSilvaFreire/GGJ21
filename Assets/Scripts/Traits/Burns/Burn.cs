using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace GGJ.Traits.Burns {
    [Serializable]
    public struct Burn {
        [TextArea]
        [TableColumnWidth(128)]
        public string message;
        [TableColumnWidth(128, resizable: false)]
        public string category;
        [TableColumnWidth(64, resizable: false)]
        public uint score;
        [SerializeReference]
        [TableColumnWidth(384)]
        public BurnFilter filter;
    }
}