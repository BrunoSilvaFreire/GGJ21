using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public static class KnowledgeX {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Knowledge> IndividualFlags() {
            return Knowledge.All.IndividualFlags();
        }
        public static IEnumerable<Knowledge> IndividualFlags(this Knowledge filter) {
            for (var i = 0; i < sizeof(Knowledge) * 8; i++) {
                var candidate = (Knowledge)(1 << i);
                if ((filter & candidate) == candidate) {
                    yield return candidate;
                }
            }
        }
    }
}