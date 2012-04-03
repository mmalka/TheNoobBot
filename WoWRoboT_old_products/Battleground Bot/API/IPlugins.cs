using System.Windows.Forms;

namespace Battleground_Bot.API
{
    /// <summary>
    /// Plugin Interface
    /// </summary>
    public abstract class IPlugins
    {
        /// <summary>
        /// UserControl (put for value null if not UserContrlol).
        /// </summary>
        public UserControl UserControl;

        internal string Name = "";

        /// <summary>
        ///  Initializes Plugin.
        ///  </summary>
        public abstract void Initialize();

        /// <summary>
        ///  Dispose Plugin.
        ///  </summary>
        public abstract void Dispose();
    }
}