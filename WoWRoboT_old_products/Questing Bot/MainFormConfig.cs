using System;
using System.IO;
using System.Windows.Forms;
using WowManager;
using WowManager.Others;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;

namespace Questing_Bot
{
    [Serializable]
    public class MainFormConfig
    {
        public bool UseMount = false;
        public int MountBar = 1;
        public int MountSlot = 1;

        public bool FarmMine = true;
        public bool FarmHerb = true;
        public bool Skinning = false;

        public bool AttackNpcInCombat;

        public string CustomClassName = "Original.cs";

        public string ProfileName = String.Empty;

        public bool UsePathFinder = true;

        public bool Talents = true;

        public bool UseSpiritHealer = false;

        public bool MailAt = false;
        public string MailAtName = "";

        public bool RegenMp;
        public int RegenMaxHp = 85;
        public int RegenMaxMp = 85;
        public int RegenMinHp = 50;
        public int RegenMinMp = 50;
        public int RegenPetMaxHp = 85;
        public int RegenPetMinHp = 50;
        public bool RegenUseFood = true;
        public bool RegenUsePetMacro;
        public bool RegenUseWater;
        public int PetBar = 1;
        public int PetSlot = 1;
        public bool RegenPet;
        public bool ReLogActive;

        public string ProfileGrinding = "";
        public string FoodNameGrinding = "";
        public string WaterNameGrinding = "";

        public string Relog = string.Empty;

        private static string FileName
        {
            get
            {
                try
                {
                    return Application.StartupPath + "\\Products\\Questing Bot\\Config\\" +
                           Others.DelSpecialChar(ObjectManager.Me.Name) + "." +
                           ObjectManager.Me.WowClass + "." + Others.DelSpecialChar(Useful.RealmName) + ".xml";
                }
                catch (Exception)
                {
                    return Application.StartupPath + "\\Products\\Questing Bot\\Config\\null.null.xml";
                }
            }
        }

        internal void Save()
        {
            try
            {
                XmlSerializerHelper.Serialize(FileName, this);
            }
            catch (Exception e)
            {
                Log.AddLog(e.ToString());
            }
        }

        internal static MainFormConfig Load()
        {
            try
            {
                if (File.Exists(FileName))
                {
                    var t = XmlSerializerHelper.Deserialize<MainFormConfig>(FileName);
                    return t;
                }
            }
            catch (Exception e)
            {
                Log.AddLog(e.ToString());
            }
            return new MainFormConfig();
        }

        internal static void Reset()
        {
            var t = new MainFormConfig();
            t.Save();
            return;
        }
    }
}
