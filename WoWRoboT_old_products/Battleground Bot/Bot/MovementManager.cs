using WowManager.Navigation;
using WowManager.Others;
using WowManager.WoW.ObjectManager;

namespace Battleground_Bot.Bot
{
    internal static class MovementManager
    {
        public static void Go()
        {
            try
            {
                if (
                    Config.Bot.Profile.SubProfiles[Config.Bot.IdProfil].Locations[
                        Others.NearestPointOfListPoints(Config.Bot.Profile.SubProfiles[Config.Bot.IdProfil].Locations,
                                                        ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) >
                    30 && !WowManager.Navigation.MovementManager.InMovement && Config.Bot.BotStarted)
                {
                    WowManager.Navigation.MovementManager.Go(
                        PathFinderManager.FindPath(
                            Config.Bot.Profile.SubProfiles[Config.Bot.IdProfil].Locations[
                                Others.NearestPointOfListPoints(
                                    Config.Bot.Profile.SubProfiles[Config.Bot.IdProfil].Locations,
                                    ObjectManager.Me.Position)]));
                }
                else if (!WowManager.Navigation.MovementManager.InMovement && Config.Bot.BotStarted)
                {
                    WowManager.Navigation.MovementManager.GoLoop(
                        Config.Bot.Profile.SubProfiles[Config.Bot.IdProfil].Locations);
                }
            }
            catch
            {
            }
        }
    }
}