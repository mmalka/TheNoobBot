namespace nManager.Plugins
{
    /// <summary>
    ///     Plugins interface
    /// </summary>
    public interface IPlugins
    {
        /// <summary>
        ///     Name of the Plugin.
        /// </summary>
        bool Loop { get; set; }

        /// <summary>
        ///     Name of the Plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Author of the Plugin, for Mouse Hover and Info Button.
        /// </summary>
        string Author { get; }

        /// <summary>
        ///     Description of the Plugin, for Mouse Hover and Info button.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     TheNoobBot's target version of this Plugin.
        /// </summary>
        string TargetVersion { get; }

        /// <summary>
        ///     Version of this Plugin.
        /// </summary>
        string Version { get; }

        /// <summary>
        ///     Return true if the Plugin is started, else false.
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        ///     Initialize Plugin.
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Dispose Plugin
        /// </summary>
        void Dispose();

        /// <summary>
        ///     Show Configuration window or Popup an error if no configurations available for the product.
        /// </summary>
        void ShowConfiguration();

        /// <summary>
        ///     Reset Configuration window or Popup an error if no configurations available for the product.
        /// </summary>
        void ResetConfiguration();

        /// <summary>
        ///     Alternative Initialize() with no logging for opening the plugin silently for General Settings purposes.
        /// </summary>
        void CheckFields();
    }
}