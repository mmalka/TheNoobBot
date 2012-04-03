using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;

namespace Smelting
{
    class Smelting
    {
        public static void Pulse()
        {
            var thread = new Thread(ThreadPulse) {Name = "Thread Smelting"};
            thread.Start();
        }

        static void ThreadPulse()
        {
            bool ignoreSmeltingZone;
            DialogResult resulMb =
    MessageBox.Show(
        nManager.Translate.Get(nManager.Translate.Id.Smelting_here),
        nManager.Translate.Get(nManager.Translate.Id.Smelting), MessageBoxButtons.YesNo,
        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (resulMb == DialogResult.Yes)
            {
                ignoreSmeltingZone = true;
            }
            else
            {
                if (NpcDB.GetNpcNearby(Npc.NpcType.Smelting).Entry <= 0)
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.Smelting_zone_not_found_in) + " 'NpcDB.xml'.");
                    nManager.Products.Products.ProductStop();
                    return;
                }
                ignoreSmeltingZone = false;
            }

            var smelting = new nManager.Wow.Bot.States.SmeltingState { IgnoreSmeltingZone = ignoreSmeltingZone };
            smelting.Run();
            Logging.Write("Smelting finished.");
            nManager.Products.Products.ProductStop();
        }
    }
}
