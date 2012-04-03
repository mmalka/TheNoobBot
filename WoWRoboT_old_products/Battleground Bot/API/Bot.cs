namespace Battleground_Bot.API
{
    public static class Bot
    {
        /// <summary>
        /// Stop the bot.
        /// </summary>
        public static void StopBot()
        {
            if (Battleground_Bot.Config.Bot.BotStarted)
                Main.WindowProduct.LaunchBot();
        }

        /// <summary>
        /// Launche the bot.
        /// </summary>
        public static void LaunchBot()
        {
            if (!Battleground_Bot.Config.Bot.BotStarted)
                Main.WindowProduct.LaunchBot(true);
        }

        /// <summary>
        /// Pause the bot.
        /// </summary>
        public static void PauseBot()
        {
            try
            {
                Battleground_Bot.Config.Bot.ForcePause = true;
            }
            catch { }
        }

        /// <summary>
        /// Stop the pause bot.
        /// </summary>
        public static void StopPauseBot()
        {
            try
            {
                Battleground_Bot.Config.Bot.ForcePause = false;
            }
            catch { }
        }

        /// <summary>
        /// Gets if bot is launched.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this bot is launched; otherwise, <c>false</c>.
        /// </value>
        public static bool IsLaunched
        {
            get
            {
                try
                {
                    return Battleground_Bot.Config.Bot.BotStarted;
                }
                catch {}

                return false;
            }
        }

        /// <summary>
        /// Get if bot is paused.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is paused; otherwise, <c>false</c>.
        /// </value>
        public static bool IsPaused
        {
            get
            {
                try
                {
                    return Battleground_Bot.Config.Bot.ForcePause;
                }
                catch { }

                return false;
            }
        }

        /// <summary>
        /// Get or set a value for enable or disable button start.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [enabled button launch bot]; otherwise, <c>false</c>.
        /// </value>
        public static bool EnabledButtonLaunchBot
        {
            get { return Main.WindowProduct.EnabledButtonStartBot; }
            set
            {
                Main.WindowProduct.EnabledButtonStartBot = value;
            }
        }
    }
}
