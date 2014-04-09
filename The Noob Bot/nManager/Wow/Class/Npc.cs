using System;
using System.Xml.Serialization;

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

        private int _entry;
        private ulong _guid;

        [XmlIgnore]
        public ulong Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _name = "NoName";

        public string InternalData { get; set; }

        public int SelectGossipOption = 1;

        public Point Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private Point _position = new Point();

        public FactionType Faction
        {
            get { return _faction; }
            set { _faction = value; }
        }

        private FactionType _faction = FactionType.Neutral;

        public NpcType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private NpcType _type = NpcType.None;

        public Enums.ContinentId ContinentId
        {
            get { return _continentId; }
            set { _continentId = value; }
        }

        private Enums.ContinentId _continentId;


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
            ArchaeologyTrainer,
            RidingTrainer,
            BattlePetTrainer,
            SmeltingForge,
            RuneForge,
            MonkTrainer,
            FlightMaster,
            SpiritHealer,
            SpiritGuide,
            Innkeeper,
            Banker,
            Battlemaster,
            Auctioneer,
            StableMaster,
            GuildBanker,
        }
    }
}