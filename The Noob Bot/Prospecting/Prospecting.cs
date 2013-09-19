using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Helpers;

namespace Prospecting
{
    internal class Prospecting
    {
        public static void Pulse()
        {
            Thread thread = new Thread(ThreadPulse) {Name = "Thread Prospecting"};
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

            ProspectingState prospecting = new ProspectingState();
            prospecting.Run();
            Logging.Write("Prospecting finished.");
            nManager.Products.Products.ProductStop();
        }
    }
}