using nManager.Wow.MemoryClass;

namespace nManager.Wow
{
    /// <summary>
    /// Memory Class
    /// </summary>
    public static class Memory
    {
        /// <summary>
        /// Process Instance (Management memory)
        /// </summary>
        public static Process WowProcess = new Process();

        /// <summary>
        /// MyHook Instance (Management memory)
        /// </summary>
        public static Hook WowMemory = new Hook();
    }
}
