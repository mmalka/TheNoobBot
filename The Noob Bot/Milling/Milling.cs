using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;

namespace Milling
{
    class Milling
    {
        public static void Pulse()
        {
            var thread = new Thread(ThreadPulse) { Name = "Thread Milling" };
            thread.Start();
        }

        static void ThreadPulse()
        {
            if (nManager.nManagerSetting.CurrentSetting.millingList.Count <= 0)
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.Please_add_items_to_mil_in__General_Settings_____Looting_____Milling_List));
                nManager.Products.Products.ProductStop();
                return;
            }

            var Milling = new nManager.Wow.Bot.States.MillingState();
            Milling.Run();
            Logging.Write("Milling finished.");
            nManager.Products.Products.ProductStop();
        }
    }
}
