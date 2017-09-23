using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Timer = nManager.Helpful.Timer;
using XmlSerializer = nManager.Helpful.XmlSerializer;

namespace nManager.Wow.Bot.States
{
    public class AutoItemCombiner : State
    {
        [Serializable]
        public class Combinable
        {
            [XmlAttribute(AttributeName = "ItemId")] public int ItemId = 0;
            [XmlAttribute(AttributeName = "PerAmount")] public int PerAmount = 0;
        }

        [Serializable]
        public class Combinables
        {
            public List<Combinable> Items = new List<Combinable>();
        }

        private static Combinables _loadedCombinables = new Combinables();
        private static Dictionary<int, int> _tempItemStock = new Dictionary<int, int>();
        private static Timer _remakeTimer = new Timer(600000);

        public override int Priority { get; set; }

        public override string DisplayName
        {
            get { return "AutoItemCombiner"; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.MakeStackOfElementalsItems)
                    return false;
                if (Usefuls.BadBottingConditions || Usefuls.ShouldFight)
                    return false;
                if (!_remakeTimer.IsReady)
                    return false;
                if (Usefuls.IsFlying && Usefuls.ContinentNameMpq == "Expansion01")
                    return false;

                if (_loadedCombinables.Items.Count <= 0)
                {
                    _loadedCombinables = XmlSerializer.Deserialize<Combinables>(Application.StartupPath + @"\Data\CombinablesDB.xml");
                    /*_loadedCombinables.Items.Sort((x, y) => x.ItemId.CompareTo(y.ItemId));
                    XmlSerializer.Serialize(Application.StartupPath + @"\Data\CombinablesDB.xml", _loadedCombinables);*/
                    if (!nManagerSetting.CurrentSetting.ActivateLootStatistics)
                    {
                        Logging.Write("The Auto ItemCombiner features works better if you have activated Loot Statistics in the settings of the bot.");
                    }
                }

                _tempItemStock = new Dictionary<int, int>();
                if (!nManagerSetting.CurrentSetting.ActivateLootStatistics)
                {
                    List<WoWItem> objectWoWItems = ObjectManager.ObjectManager.GetObjectWoWItem();
                    for (int i = 0; i < objectWoWItems.Count; i++)
                    {
                        WoWItem item = objectWoWItems[i];
                        if (!item.IsValid || item.Entry < 1 || _tempItemStock.ContainsKey(item.Entry))
                            continue;
                        int count = ItemsManager.GetItemCount(item.Entry);
                        if (count <= 0)
                            continue;
                        _tempItemStock.Add(item.Entry, count);
                    }
                }
                else
                {
                    _tempItemStock = Others.ItemStock;
                }
                for (int i = _loadedCombinables.Items.Count - 1; i > 0; i--)
                {
                    var combinable = _loadedCombinables.Items[i];
                    if (_tempItemStock.ContainsKey(combinable.ItemId) && _tempItemStock[combinable.ItemId] > combinable.PerAmount)
                    {
                        return true;
                    }
                }
                _remakeTimer.Reset();
                return false;
            }
        }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        public override void Run()
        {
            Logging.Write("AutoItemCombiner is now running, trying to combines all items...");
            if (Usefuls.IsFlying)
            {
                MountTask.DismountMount();
                Thread.Sleep(400);
                MountTask.DismountMount();
                Thread.Sleep(400);
            }
            for (int i = _loadedCombinables.Items.Count - 1; i > 0; i--)
            {
                if (Usefuls.BadBottingConditions || Usefuls.ShouldFight)
                    return;
                var combinable = _loadedCombinables.Items[i];
                if (!_tempItemStock.ContainsKey(combinable.ItemId) || _tempItemStock[combinable.ItemId] <= combinable.PerAmount) continue;
                int remainder;
                int amountToCombine = System.Math.DivRem(ItemsManager.GetItemCount(combinable.ItemId), combinable.PerAmount, out remainder);
                Timer t = new Timer(amountToCombine*1000);
                while (System.Math.DivRem(ItemsManager.GetItemCount(combinable.ItemId), combinable.PerAmount, out remainder) > 0)
                {
                    if (Usefuls.BadBottingConditions || Usefuls.ShouldFight)
                        return;
                    ItemsManager.UseItem(combinable.ItemId);
                    Thread.Sleep(50 + Usefuls.Latency);
                    if (t.IsReady)
                        break;
                }
            }
            _remakeTimer.Reset();
        }
    }
}