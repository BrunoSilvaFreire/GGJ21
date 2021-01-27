using System;
namespace Common {
    [Flags]
    public enum CollisionMode : byte {
        Collision = 1 << 0,
        Trigger = 1 << 1,
        ManualQuery = 1 << 2
    }
}