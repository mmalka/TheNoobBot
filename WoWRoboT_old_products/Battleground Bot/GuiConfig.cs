using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using WowManager;
using WowManager.Others;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;

namespace Battleground_Bot
{
    [Serializable]
    public class GuiConfig
    {
        public readonly bool AlteracValley;
        public readonly bool ArathiBasin;
        public readonly bool Bfg;
        public readonly string CustomClass = "Original.cs";
        public List<string> EnablePlugins = new List<string>();
        public bool EyeOfTheStorm;
        public bool Ioc;
        public bool Loot = true;
        public int MountBar = 1;
        public int MountSlot = 1;
        public bool PathFinder = true;
        public bool RandomBg = true;
        public string Reloger = "";
        public bool Requeue;
        public bool Sota;
        public bool Tp;
        public bool UseMount;
        public bool WarsongGuich;

        internal GuiConfig()
        {
        }

        internal GuiConfig(bool randomBg, bool warsongGuich, bool eyeOfTheStorm, bool arathiBasin, bool alteracValley,
                           bool sota, bool ioc, bool tp, bool bfg, string customClass, bool pathFinder, bool useMount,
                           int mountBar, int mountSlot, string reloger, bool loot, bool requeue,
                           List<string> enablePlugins)
        {
            RandomBg = randomBg;
            WarsongGuich = warsongGuich;
            EyeOfTheStorm = eyeOfTheStorm;
            ArathiBasin = arathiBasin;
            AlteracValley = alteracValley;
            Sota = sota;
            Ioc = ioc;
            Tp = tp;
            Bfg = bfg;
            CustomClass = customClass;
            PathFinder = pathFinder;
            MountBar = mountBar;
            MountSlot = mountSlot;
            UseMount = useMount;
            Reloger = reloger;
            Loot = loot;
            Requeue = requeue;
            EnablePlugins = enablePlugins;
        }

        internal static string FileName
        {
            get
            {
                try
                {
                    return Application.StartupPath + "\\Products\\Battleground Bot\\Config\\" +
                           Others.DelSpecialChar(ObjectManager.Me.Name) + "." +
                           ObjectManager.Me.WowClass + "." + Others.DelSpecialChar(Useful.RealmName) + ".xml";
                }
                catch (Exception)
                {
                    return Application.StartupPath + "\\Products\\Battleground Bot\\Config\\null.null.xml";
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

        internal static GuiConfig Load()
        {
            try
            {
                if (File.Exists(FileName))
                {
                    var t = XmlSerializerHelper.Deserialize<GuiConfig>(FileName);
                    return t;
                }
            }
            catch (Exception e)
            {
                Log.AddLog(e.ToString());
            }
            return new GuiConfig();
        }

        internal static GuiConfig Reset()
        {
            var t = new GuiConfig();
            t.Save();
            return t;
        }
    }
}