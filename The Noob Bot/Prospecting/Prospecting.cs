using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;

namespace Prospecting
{
    internal class Prospecting
    {
        public static void Pulse()
        {
            var thread = new Thread(ThreadPulse) {Name = "Thread Prospecting"};
            thread.Start();
        }

        private static void ThreadPulse()
        {
            if (nManager.nManagerSetting.CurrentSetting.MineralsToProspect.Count <= 0)
            {
                MessageBox.Show(
                    nManager.Translate.Get(
                        nManager.Translate.Id
                                .Please_add_items_to_prospect_in__General_Settings_____Looting_____Prospecting_List));
                nManager.Products.Products.ProductStop();
                return;
            }

            var prospecting = new nManager.Wow.Bot.States.ProspectingState();
            prospecting.Run();
            Logging.Write("Prospecting finished.");
            nManager.Products.Products.ProductStop();
        }
    }
}