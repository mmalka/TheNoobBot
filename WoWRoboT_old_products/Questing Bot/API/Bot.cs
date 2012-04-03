using WowManager.MiscEnums;

namespace Questing_Bot.API
{
    public static class Bot
    {
        /// <summary>
        /// Stop the bot.
        /// </summary>
        public static void StopBot()
        {
            if (Questing_Bot.Bot.Config.Bot.BotIsActive)
                Main.WindowProduct.LaunchBot();
        }

        /// <summary>
        /// Launche the bot.
        /// </summary>
        public static void LaunchBot()
        {
            if (!Questing_Bot.Bot.Config.Bot.BotIsActive)
                Main.WindowProduct.LaunchBot();
        }

        /// <summary>
        /// Pause the bot.
        /// </summary>
        public static void PauseBot()
        {
            try
            {
                Questing_Bot.Bot.Config.Bot.ForcePause = true;
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
                Questing_Bot.Bot.Config.Bot.ForcePause = false;
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
                    return Questing_Bot.Bot.Config.Bot.BotIsActive;
                }
                catch { }

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
                    return Questing_Bot.Bot.Config.Bot.ForcePause;
                }
                catch { }

                return false;
            }
        }
    }
}
