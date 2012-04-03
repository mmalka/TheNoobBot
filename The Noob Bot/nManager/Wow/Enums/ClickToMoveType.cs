namespace nManager.Wow.Enums
{
    public enum ClickToMoveType
    {
        FaceTarget = 0x1,
        Face = 0x2,
        /// <summary>
        /// Will throw a UI error. Have not figured out how to avoid it!
        /// </summary>
        Stop_ThrowsException = 0x3,
        Move = 0x4,
        NpcInteract = 0x5,
        Loot = 0x6,
        ObjInteract = 0x7,
        FaceOther = 0x8,
        Skin = 0x9,
        AttackPosition = 0xA,
        AttackGuid = 0xB,
        ConstantFace = 0xC,
        None = 0xD,

        Attack = 0x10,
        Idle = 0x13,
    }
}
