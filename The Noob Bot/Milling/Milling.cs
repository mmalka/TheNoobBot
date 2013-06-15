using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;

namespace Milling
{
    internal class Milling
    {
        public static void Pulse()
        {
            var thread = new Thread(ThreadPulse) {Name = "Thread Milling"};
            thread.Start();
        }

        private static void ThreadPulse()
        {
            if (nManager.nManagerSetting.CurrentSetting.HerbsToBeMilled.Count <= 0)
            {
                MessageBox.Show(
                    nManager.Translate.Get(
                        nManager.Translate.Id.Please_add_items_to_mil_in__General_Settings_____Looting_____Milling_List));
                nManager.Products.Products.ProductStop();
                return;
            }

            var milling = new nManager.Wow.Bot.States.MillingState();
            milling.Run();
            Logging.Write("Milling finished.");
            nManager.Products.Products.ProductStop();
        }
    }
}