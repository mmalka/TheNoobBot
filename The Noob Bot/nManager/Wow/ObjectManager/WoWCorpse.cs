using System;
using nManager.Helpful;
using nManager.Wow.Patchables;

namespace nManager.Wow.ObjectManager
{
    public class WoWCorpse : WoWObject
    {
        public WoWCorpse(uint address)
            : base(address)
        {
        }

        public T GetDescriptor<T>(Descriptors.CorpseFields field) where T : struct
        {
            try
            {
                return GetDescriptor<T>((uint) field);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetDescriptor<T>(Descriptors.CorpseFields field): " + e);
                return default(T);
            }
        }
    }
}