namespace nManager.Wow.Enums
{
    public enum GuidType : byte
    {
        Null,
        Uniq,
        Player,
        Item,
        WorldTransaction,
        StaticDoor,
        Transport,
        Conversation,
        Creature,
        Vehicle,
        Pet,
        GameObject,
        DynamicObject,
        AreaTrigger,
        Corpse,
        LootObject,
        SceneObject,
        Scenario,
        AIGroup,
        DynamicDoor,
        ClientActor,
        Vignette,
        CallForHelp,
        AIResource,
        AILock,
        AILockTicket,
        ChatChannel,
        Party,
    }

    public enum GuidSubType : byte
    {
        None = 0
    }
}