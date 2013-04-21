using System;

namespace nManager.Wow.Enums
{
    [Flags]
    public enum CGWorldFrameHitFlags : uint
    {
        HitTestNothing = 0x0,

        /// <summary>
        /// Models' bounding, ie. where you can't walk on a model. (Trees, smaller structures etc.)
        /// </summary>
        HitTestBoundingModels = 0x1,

        /// <summary>
        /// Structures like big buildings, Orgrimmar etc.
        /// </summary>
        HitTestWMO = 0x10,

        /// <summary>
        /// Used in ClickTerrain.
        /// </summary>
        HitTestUnknown = 0x40,

        /// <summary>
        /// The terrain.
        /// </summary>
        HitTestGround = 0x100,

        /// <summary>
        /// Tested on water - should work on lava and other liquid as well.
        /// </summary>
        HitTestLiquid = 0x10000,

        /// <summary>
        /// This flag works for liquid as well, but it also works for something else that I don't know (this hit while the liquid flag didn't hit) - called constantly by WoW.
        /// </summary>
        HitTestUnknown2 = 0x20000,

        /// <summary>
        /// Hits on movable objects - tested on UC elevator doors.
        /// </summary>
        HitTestMovableObjects = 0x100000,

        HitTestLOS = HitTestWMO | HitTestBoundingModels | HitTestMovableObjects,

        HitTestAll =
            HitTestWMO | HitTestBoundingModels | HitTestNothing | HitTestLiquid | HitTestGround | HitTestUnknown,

        HitTestAllButLiquid =
            HitTestWMO | HitTestBoundingModels | HitTestNothing | HitTestGround | HitTestUnknown,

        HitTestGroundAndStructures = HitTestLOS | HitTestGround
    }
}