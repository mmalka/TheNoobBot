using System;

namespace nManager.Wow.Enums
{
    [Flags]
    public enum MovementFlags : uint
    {
        None = 0x0,
        MovingForward = 0x1,
        MovingBackwards = 0x2,
        StrafingLeft = 0x4,
        StrafingRight = 0x8,
        StrafeMask = 0xC,
        TurningLeft = 0x10,
        TurningRight = 0x20,
        TurnMask = 0x30,
        PitchUp = 0x40,
        PitchDown = 0x80,
        PitchMask = 0xc0,
        Walk = 0x100,
        Levitating = 0x200,
        Root = 0x400, // Impairements effect. Root, Stun, or simply when there is a loading screen.
        Falling = 0x800, // Script_IsFalling
        FallingFar = 0x1000, // What is this exactly ? "Going to die from the fall" or "far from the floor at this moment"
        FallMask = 0x1800,
        PendingStop = 0x2000,
        PendingStrafeStop = 0x4000,
        PendingForward = 0x8000,
        PendingBackward = 0x10000,
        PendingStrafeLeft = 0x20000,
        PendingStrafeRight = 0x40000,
        PendingRoot = 0x80000,
        Swimming = 0x100000, // Script_IsSwimming
        Ascending = 0x200000,
        Descending = 0x400000,
        CanFly = 0x800000,
        Flying = 0x1000000, // Script_IsFlying
        SplineElevation = 0x2000000,
        SplineEnabled = 0x4000000,
        WaterWalking = 0x8000000,
        SafeFall = 0x10000000,
        Hover = 0x20000000,
        PlayerFlag = 0x80000000,
        MotionMask = 0xff,
        MoveMask = 0x3f,
    }
}