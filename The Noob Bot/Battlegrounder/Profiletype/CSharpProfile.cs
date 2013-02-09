using nManager.Wow.Helpers;

namespace Battlegrounder.Profiletype
{
    internal class CSharpProfile
    {
        public static bool CSharpProfileNow(string profileTypeScriptName)
        {
            if (CustomProfile.IsAliveCustomProfile)
                CustomProfile.ResetCustomProfile();
            else
                CustomProfile.LoadCustomProfile(true, profileTypeScriptName);
            return CustomProfile.IsAliveCustomProfile;
        }
    }
}