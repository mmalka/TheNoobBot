namespace Battleground_Bot.API
{
    public static class Config
    {
        /// <summary>
        /// Reset the config.
        /// </summary>
        public static void ResetConfig()
        {
            GuiConfig.Reset();
            Main.WindowProduct.SaveConfig();
        }

        /// <summary>
        /// Gets or sets the bot Config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public static GuiConfig ConfigBot
        {
            get
            {
                return Main.WindowProduct.SaveConfig();
            }
            set
            {
                value.Save();
                Main.WindowProduct.LoadConfig();
            }
        }

        /// <summary>
        /// Save the config.
        /// </summary>
        public static void SaveConfig()
        {
            Main.WindowProduct.SaveConfig();
        }

        /// <summary>
        /// Load the config.
        /// </summary>
        public static void LoadConfig()
        {
            Main.WindowProduct.LoadConfig();

        }
    }
}
