namespace nManager.Wow.Enums
{
    public enum ClickToMoveType
    {
        None = 0x0,
        FaceTarget = 0x2,
        Face = 0x3,

        /// <summary>
        /// Will throw a UI error. Have not figured out how to avoid it!
        /// </summary>
        Stop_ThrowsException = 0x4,
        Move = 0x5,
        NpcInteract = 0x6,
        Loot = 0x7,
        ObjInteract = 0x8,
        FaceOther = 0x9,
        Skin = 0xA,
        AttackPosition = 0xB,
        AttackGuid = 0xC,
        ConstantFace = 0xD,

        Attack = 0x10,
        Idle = 0x13,
    }
}