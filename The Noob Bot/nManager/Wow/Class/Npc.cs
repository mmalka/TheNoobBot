using System;

namespace nManager.Wow.Class
{
    [Serializable]
    public class Npc
    {
        public int Entry
        {
            get { return _entry; }
            set { _entry = value; }
        }
        int _entry;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        string _name = "";

        public Point Position
        {
            get { return _position; }
            set { _position = value; }
        }
        Point _position = new Point();

        public FactionType Faction
        {
            get { return _faction; }
            set { _faction = value; }
        }
        FactionType _faction = FactionType.Neutral;

        public NpcType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        NpcType _type = NpcType.None;

        public Enums.ContinentId ContinentId
        {
            get { return _continentId; }
            set { _continentId = value; }
        }
        Enums.ContinentId _continentId;



        

        [Serializable]
        public enum FactionType
        {
            Neutral,
            Horde,
            Alliance,
        }

        [Serializable]
        public enum NpcType
        {
            None,
            Vendor,
            Repair,
            AuctionHouse,
            Mailbox,
            DruidTrainer,
            RogueTrainer,
            WarriorTrainer,
            PaladinTrainer,
            HunterTrainer,
            PriestTrainer,
            DeathKnightTrainer,
            ShamanTrainer,
            MageTrainer,
            WarlockTrainer,
            AlchemyTrainer,
            BlacksmithingTrainer,
            EnchantingTrainer,
            EngineeringTrainer,
            HerbalismTrainer,
            InscriptionTrainer,
            JewelcraftingTrainer,
            LeatherworkingTrainer,
            TailoringTrainer,
            MiningTrainer,
            SkinningTrainer,
            CookingTrainer,
            FirstAidTrainer,
            FishingTrainer,
            ArchaeologyTrainers,
            RidingTrainer,
            Smelting,
        }
    }
}
