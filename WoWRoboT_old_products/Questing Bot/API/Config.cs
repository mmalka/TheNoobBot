namespace Questing_Bot.API
{
    public static class Config
    {
        /// <summary>
        /// Reset the config.
        /// </summary>
        public static void ResetConfig()
        {
            MainFormConfig.Reset();
            Main.WindowProduct.LoadConfig();
        }

        /// <summary>
        /// Gets or sets the bot Config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public static MainFormConfig ConfigBot
        {
            get
            {
                Main.WindowProduct.SaveConfig();
                return MainFormConfig.Load();
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
