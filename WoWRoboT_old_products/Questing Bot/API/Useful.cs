namespace Questing_Bot.API
{
    public static class Useful
    {
        public static Questing_Bot.Bot.Bot BotCurrentSession
        {
            get { return Questing_Bot.Bot.Config.Bot; }
            set { Questing_Bot.Bot.Config.Bot = value; }
        }

        public static void GoToTown()
        {
            Questing_Bot.Bot.Config.Bot.ForceGoToTown = true;
        }

        public static void GoToTrainers()
        {
            Questing_Bot.Bot.Config.Bot.ForceGoToTrainers = true;
        }

        public static bool DisableLoot { get { return Questing_Bot.Bot.Config.Bot.DisableLoot; } set { Questing_Bot.Bot.Config.Bot.DisableLoot = value; } }

        public static bool DisableRepairAndVendor { get { return Questing_Bot.Bot.Config.Bot.DisableRepairAndVendor; } set { Questing_Bot.Bot.Config.Bot.DisableRepairAndVendor = value; } }
    }
}
