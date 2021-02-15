using UnityEngine;
namespace GGJ.Common {
    public interface IIndexable {
        int Width { get; }
    }

    public interface IBoundedIndexable : IIndexable {
        Vector2Int Min { get; }

        Vector2Int Max { get; }
    }

   
}