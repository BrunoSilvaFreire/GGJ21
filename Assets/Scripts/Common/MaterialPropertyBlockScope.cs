using System;
using UnityEngine;
namespace Common {
    public class MaterialPropertyBlockScope : IDisposable {
        public MaterialPropertyBlockScope(Renderer renderer) {
            Block = new MaterialPropertyBlock();
            Renderer = renderer;
            renderer.GetPropertyBlock(Block);
        }

        public void Dispose() {
            Renderer.SetPropertyBlock(Block);
        }

        public MaterialPropertyBlock Block { get; }

        public Renderer Renderer { get; }
    }
}