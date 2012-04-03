using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WowManager;
using WowManager.MiscEnums;
using WowManager.MiscStructs;
using WowManager.Navigation;
using WowManager.Others;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;

namespace Battleground_Bot.Bot
{
    internal static class GetProfile
    {
        private static int _continentId = -1;

        internal static void Go()
        {
            if (Useful.ContinentId == (int) ContinentId.AV || Useful.ContinentId == (int) ContinentId.AB ||
                Useful.ContinentId == (int) ContinentId.EOTS || Useful.ContinentId == (int) ContinentId.WSG ||
                Useful.ContinentId == (int) ContinentId.SOTA || Useful.ContinentId == (int) ContinentId.IOC ||
                Useful.ContinentId == (int) ContinentId.TP || Useful.ContinentId == (int) ContinentId.BFG)
            {
                if (Useful.ContinentId != _continentId || Config.Bot.IdProfil < 0)
                {
                    try
                    {
                        // Selecte Profile
                        if (Useful.ContinentId == (int) ContinentId.AV)
                        {
                            Config.Bot.Profile = LoadProfile(ContinentId.AV);
                            Log.AddLog(Translation.GetText(Translation.Text.Load_profile_for) + " Alterac Valley.");
                        }
                        else if (Useful.ContinentId == (int) ContinentId.AB)
                        {
                            Config.Bot.Profile = LoadProfile(ContinentId.AB);
                            Log.AddLog(Translation.GetText(Translation.Text.Load_profile_for) + " Arathi Basin.");
                        }
                        else if (Useful.ContinentId == (int) ContinentId.EOTS)
                        {
                            Config.Bot.Profile = LoadProfile(ContinentId.EOTS);
                            Log.AddLog(Translation.GetText(Translation.Text.Load_profile_for) + " Eye of the Storm.");
                        }
                        else if (Useful.ContinentId == (int) ContinentId.WSG)
                        {
                            Config.Bot.Profile = LoadProfile(ContinentId.WSG);
                            Log.AddLog(Translation.GetText(Translation.Text.Load_profile_for) + " WarSong Guich.");
                        }
                        else if (Useful.ContinentId == (int) ContinentId.SOTA)
                        {
                            Config.Bot.Profile = LoadProfile(ContinentId.SOTA);
                            Log.AddLog(Translation.GetText(Translation.Text.Load_profile_for) + " Strand of the Ancients.");
                        }
                        else if (Useful.ContinentId == (int) ContinentId.IOC)
                        {
                            Config.Bot.Profile = LoadProfile(ContinentId.IOC);
                            Log.AddLog(Translation.GetText(Translation.Text.Load_profile_for) + " Isle of Conquest.");
                        }
                        else if (Useful.ContinentId == (int) ContinentId.TP)
                        {
                            Config.Bot.Profile = LoadProfile(ContinentId.TP);
                            Log.AddLog(Translation.GetText(Translation.Text.Load_profile_for) + " Twin Peaks.");
                        }
                        else if (Useful.ContinentId == (int) ContinentId.BFG)
                        {
                            Config.Bot.Profile = LoadProfile(ContinentId.BFG);
                            Log.AddLog(Translation.GetText(Translation.Text.Load_profile_for) + " The Battle for Gilnea.");
                        }

                        // Selecte SubProfile ID
                        Config.Bot.IdProfil = 0;
                        for (int i = 0; i <= Config.Bot.Profile.SubProfiles.Count - 1; i++)
                        {
                            if (Config.Bot.Profile.SubProfiles[i].PlayerFaction.ToLower() == "all" ||
                                Config.Bot.Profile.SubProfiles[i].PlayerFaction.ToLower() ==
                                ObjectManager.Me.PlayerFaction.ToLower())
                            {
                                Config.Bot.IdProfil = i;
                                Log.AddLog(Translation.GetText(Translation.Text.Select_SubProfile_id) + " " + i + " " + Translation.GetText(Translation.Text.for_) + " " +
                                           Config.Bot.Profile.SubProfiles[i].PlayerFaction);
                            }
                        }

                        // BlackList
                        Config.Bot.BlackList = new List<ulong>();
                        Config.Bot.BlackList.AddRange(Config.Bot.Profile.SubProfiles[Config.Bot.IdProfil].BlackListGuid);

                        // Create path if HotSpot
                        if (Config.Bot.Profile.SubProfiles[Config.Bot.IdProfil].Hotspots)
                        {
                            var tempsPointHs = new List<Point>();
                            tempsPointHs.AddRange(Config.Bot.Profile.SubProfiles[Config.Bot.IdProfil].Locations);
                            Config.Bot.Profile.SubProfiles[Config.Bot.IdProfil].Locations.Clear();

                            for (int i = 0; i <= tempsPointHs.Count - 1 && Config.Bot.BotStarted; i++)
                            {
                                int iLast = i - 1;
                                if (iLast < 0)
                                    iLast = tempsPointHs.Count - 1;
                                Log.AddLog(Translation.GetText(Translation.Text.Create_points_HotSpot) + " " + iLast + " " + Translation.GetText(Translation.Text.to_HotSpot) + " " + i);
                                List<Point> points = PathFinderManager.FindPath(tempsPointHs[iLast], tempsPointHs[i]);
                                Config.Bot.Profile.SubProfiles[Config.Bot.IdProfil].Locations.AddRange(points);
                            }
                            Config.Bot.Profile.SubProfiles[Config.Bot.IdProfil].Hotspots = false;
                        }

                        _continentId = Useful.ContinentId;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Load Bg profile error: " + e.ToString());
                    }
                }
            }
        }

        static Profile.Profile LoadProfile(ContinentId continentId)
        {
            return XmlSerializerHelper.Deserialize<Profile.Profile>(Application.StartupPath + "\\Products\\Battleground Bot\\Profiles\\" + continentId.ToString() + ".xml");
        }
    }
}