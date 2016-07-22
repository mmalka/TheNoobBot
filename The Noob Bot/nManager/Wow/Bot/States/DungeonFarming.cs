using System.Collections.Generic;
using System.Windows.Forms;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;

namespace nManager.Wow.Bot.States
{
    public class DungeonFarming : State
    {
        private List<Instance> _instanceList;

        public override string DisplayName
        {
            get { return "DungeonFarming"; }
        }

        public override int Priority { get; set; }

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoading ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.InCombat &&
                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) ||
                    !Products.Products.IsStarted)
                    return false;

                if (_instanceList == null)
                    _instanceList = XmlSerializer.Deserialize<List<Instance>>(Application.StartupPath + "\\Data\\DfInstanceList.xml");

                if (_instanceList.Count > 0)
                {
                    // check stuffs
                }

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
        }
    }
}