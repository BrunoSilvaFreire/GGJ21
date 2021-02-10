using System;
namespace GGJ.Traits.Knowledge {
    [Flags]
    public enum Knowledge : ushort {
        None = 0,
        MoveLeft = 1 << 0,
        MoveRight = 1 << 1,
        Jump = 1 << 2,
        LookUp = 1 << 3,
        LookDown = 1 << 4,
        Glide = 1 << 5,
        Dodge = 1 << 6,
        Attack = 1 << 7,
        WallJump = 1 << 8,
        SuperJump = 1 << 9,
        Roll = 1 << 10,
        MoveHorizontally = MoveLeft | MoveRight,
        Platform = MoveHorizontally | Jump,
        All = 0b0000011111111111
    }
}