using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;

namespace Disenchanting
{
    class Disenchanting
    {
        public static void Pulse()
        {
            var thread = new Thread(ThreadPulse) { Name = "Thread Disenchanting" };
            thread.Start();
        }

        static void ThreadPulse()
        {
            if (nManager.nManagerSetting.CurrentSetting.DisenchantingList.Count <= 0)
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.Please_add_items_at_prospected_in__General_Settings_____Looting_____Disenchanting_List));
                nManager.Products.Products.ProductStop();
                return;
            }

            var Disenchanting = new nManager.Wow.Bot.States.DisenchantingState();
            Disenchanting.Run();
            Logging.Write("Disenchanting finished.");
            nManager.Products.Products.ProductStop();
        }
    }
}
