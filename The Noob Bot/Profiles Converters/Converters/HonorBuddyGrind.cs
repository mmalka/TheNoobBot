using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Grinder.Profile;
using nManager.Helpful;
using nManager.Wow.Class;

namespace Profiles_Converters.Converters
{
    public class HonorBuddyGrind
    {
        public static bool IsHonorBuddyGrindProfile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    var text = Others.ReadFile(path);
                    if (text.Contains("<HBProfile") && text.Contains("<GrindArea>"))
                        return true;
                }
                else
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.File_not_found) + ".");
                }
            }
            catch
            {
            }
            return false;
        }

        public static bool Convert(string path)
        {
            try
            {
                if (IsHonorBuddyGrindProfile(path))
                {
                    var _profile = new GrinderProfile();

                    #region LoadProfileBuddy

                    var xml = XElement.Load(path);

                    bool subProfile =
                        xml.Elements().Any(child => child.Name.ToString().ToLower() == "SubProfile".ToLower());
                    foreach (XElement child in xml.Elements())
                    {
                        if (child.Name.ToString().ToLower() == "SubProfile".ToLower() || !subProfile) // SubProfile
                        {
                            if (subProfile || _profile.GrinderZones.Count <= 0)
                            {
                                _profile.GrinderZones.Add(new GrinderZone {Hotspots = true});
                                XElement child2;
                                child2 = subProfile ? new XElement(child) : new XElement(xml);

                                foreach (XElement childSubProfile in child2.Elements())
                                {
                                    if (childSubProfile.Name == "Name" && _profile.GrinderZones.Any())
                                    {
                                        _profile.GrinderZones[_profile.GrinderZones.Count() - 1].Name =
                                            childSubProfile.Value;
                                    }

                                    if (childSubProfile.Name == "MinLevel" && _profile.GrinderZones.Any())
                                    {
                                        _profile.GrinderZones[_profile.GrinderZones.Count() - 1].MinLevel = Others.ToUInt32(childSubProfile.Value);
                                    }

                                    if (childSubProfile.Name == "MaxLevel" && _profile.GrinderZones.Any())
                                    {
                                        _profile.GrinderZones[_profile.GrinderZones.Count() - 1].MaxLevel =
                                            Others.ToUInt32(childSubProfile.Value);
                                    }

                                    if (childSubProfile.Name == "TargetMinLevel" && _profile.GrinderZones.Any())
                                    {
                                        _profile.GrinderZones[_profile.GrinderZones.Count() - 1].MinTargetLevel
                                            = Others.ToUInt32(childSubProfile.Value);
                                    }

                                    if (childSubProfile.Name == "TargetMaxLevel" && _profile.GrinderZones.Any())
                                    {
                                        _profile.GrinderZones[_profile.GrinderZones.Count() - 1].MaxTargetLevel
                                            = Others.ToUInt32(childSubProfile.Value);
                                    }
                                    /*
                                    if (childSubProfile.Name == "Mailboxes" && _profile.GrinderZones.Any())
                                    {
                                        _profile.GrinderZones[_profile.GrinderZones.Count() - 1].Mailboxes =
                                            new List<nManager.MiscStructs.Point>();

                                        foreach (XElement childMailBoxs in childSubProfile.Elements())
                                        {
                                            if (childMailBoxs.Name == "Mailbox")
                                            {
                                                XAttribute x = childMailBoxs.Attribute("X");
                                                if (x == null)
                                                    x = childMailBoxs.Attribute("x");
                                                XAttribute y = childMailBoxs.Attribute("Y");
                                                if (y == null)
                                                    y = childMailBoxs.Attribute("y");
                                                XAttribute z = childMailBoxs.Attribute("Z");
                                                if (z == null)
                                                    z = childMailBoxs.Attribute("z");

                                                if (x != null)
                                                {
                                                    if (y != null)
                                                    {
                                                        if (z != null)
                                                        {
                                                            var pT =
                                                                new nManager.MiscStructs.Point(
                                                                   Others.ToSingle(x.Value),
                                                                   Others.ToSingle(y.Value),
                                                                   Others.ToSingle(z.Value));
                                                            _profile.GrinderZones[
                                                                _profile.GrinderZones.Count() - 1].Mailboxes.Add(pT);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    } 
                                    

                                    if (childSubProfile.Name == "Vendors" && _profile.GrinderZones.Any())
                                    {
                                        _profile.GrinderZones[_profile.GrinderZones.Count() - 1].Vendors =
                                            new List<Point>();

                                        foreach (XElement childVendors in childSubProfile.Elements())
                                        {
                                            if (childVendors.Name == "Vendor")
                                            {
                                                XAttribute x = childVendors.Attribute("X");
                                                if (x == null)
                                                    x = childVendors.Attribute("x");
                                                XAttribute y = childVendors.Attribute("Y");
                                                if (y == null)
                                                    y = childVendors.Attribute("y");
                                                XAttribute z = childVendors.Attribute("Z");
                                                if (z == null)
                                                    z = childVendors.Attribute("z");

                                                if (x != null)
                                                {
                                                    if (y != null)
                                                    {
                                                        if (z != null)
                                                        {
                                                            var pT =
                                                                new nManager.MiscStructs.Point(
                                                                   Others.ToSingle(x.Value),
                                                                   Others.ToSingle(y.Value),
                                                                   Others.ToSingle(z.Value));
                                                            _profile.GrinderZones[
                                                                _profile.GrinderZones.Count() - 1].Vendors.Add(pT);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    */

                                    if (childSubProfile.Name.ToString().ToLower() == "Blackspots".ToLower() &&
                                        _profile.GrinderZones.Any())
                                    {
                                        foreach (XElement childVendors in childSubProfile.Elements())
                                        {
                                            if (childVendors.Name.ToString().ToLower() == "Blackspot".ToLower())
                                            {
                                                XAttribute x = childVendors.Attribute("X") ?? childVendors.Attribute("x");
                                                XAttribute y = childVendors.Attribute("Y") ?? childVendors.Attribute("y");
                                                XAttribute z = childVendors.Attribute("Z") ?? childVendors.Attribute("z");

                                                XAttribute r = childVendors.Attribute("Radius") ?? childVendors.Attribute("radius");

                                                if (x != null)
                                                {
                                                    if (y != null)
                                                    {
                                                        if (z != null)
                                                        {
                                                            if (r != null)
                                                            {
                                                                var pT =
                                                                    new Point(
                                                                        Others.ToSingle(x.Value),
                                                                        Others.ToSingle(y.Value),
                                                                        Others.ToSingle(z.Value));
                                                                _profile.GrinderZones[
                                                                    _profile.GrinderZones.Count() - 1].BlackListRadius.
                                                                                                       Add(new GrinderBlackListRadius
                                                                                                           {
                                                                                                               Position
                                                                                                                   =
                                                                                                                   pT,
                                                                                                               Radius
                                                                                                                   =
                                                                                                                   System
                                                                                                               .Convert
                                                                                                               .ToSingle
                                                                                                               (
                                                                                                                   r
                                                                                                                       .Value
                                                                                                                       .Replace
                                                                                                                       (".",
                                                                                                                        ","))
                                                                                                           });
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (childSubProfile.Name.ToString().ToLower() == "GrindArea".ToLower() &&
                                        _profile.GrinderZones.Any())
                                    {
                                        foreach (XElement childGrindArea in childSubProfile.Elements())
                                        {
                                            if (childGrindArea.Name == "TargetMinLevel" &&
                                                _profile.GrinderZones.Any())
                                            {
                                                _profile.GrinderZones[_profile.GrinderZones.Count() - 1].
                                                    MinTargetLevel = Others.ToUInt32(childGrindArea.Value);
                                            }

                                            if (childGrindArea.Name == "TargetMaxLevel" &&
                                                _profile.GrinderZones.Any())
                                            {
                                                _profile.GrinderZones[_profile.GrinderZones.Count() - 1].
                                                    MaxTargetLevel = Others.ToUInt32(childGrindArea.Value);
                                            }

                                            if (childGrindArea.Name == "Factions" && _profile.GrinderZones.Any())
                                            {
                                                _profile.GrinderZones[_profile.GrinderZones.Count() - 1].TargetFactions
                                                    = new List<uint>();
                                                string tempsfaction = childGrindArea.Value;

                                                if (tempsfaction.Replace(" ", "").Length > 0)
                                                {
                                                    string[] factionTempsString =
                                                        tempsfaction.Replace("  ", " ")
                                                                    .Split(' ');
                                                    foreach (string t in factionTempsString)
                                                    {
                                                        try
                                                        {
                                                            if (t != "")
                                                                _profile.GrinderZones[
                                                                    _profile.GrinderZones.Count() - 1].TargetFactions
                                                                                                      .Add(
                                                                                                          System.Convert
                                                                                                                .ToUInt32
                                                                                                              (t));
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }
                                                }
                                            }

                                            if (childGrindArea.Name == "Hotspots" && _profile.GrinderZones.Any())
                                            {
                                                _profile.GrinderZones[_profile.GrinderZones.Count() - 1].
                                                    Points = new List<Point>();
                                                foreach (XElement childHotspots in childGrindArea.Elements())
                                                {
                                                    if (childHotspots.Name == "Hotspot")
                                                    {
                                                        XAttribute x = childHotspots.Attribute("X") ?? childHotspots.Attribute("x");
                                                        XAttribute y = childHotspots.Attribute("Y") ?? childHotspots.Attribute("y");
                                                        XAttribute z = childHotspots.Attribute("Z") ?? childHotspots.Attribute("z");

                                                        if (x != null)
                                                        {
                                                            if (y != null)
                                                            {
                                                                if (z != null)
                                                                {
                                                                    var pT =
                                                                        new Point(
                                                                            Others.ToSingle(x.Value),
                                                                            Others.ToSingle(y.Value),
                                                                            Others.ToSingle(z.Value));
                                                                    _profile.GrinderZones[
                                                                        _profile.GrinderZones.Count() - 1].Points.Add
                                                                        (pT);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (!subProfile)
                                    break;
                            }
                        }
                    }

                    #endregion LoadProfileBuddy

                    var fileName = Path.GetFileNameWithoutExtension(path);
                    if (XmlSerializer.Serialize(Application.StartupPath + "\\Profiles\\Grinder\\" + fileName + ".xml",
                                                _profile))
                    {
                        Logging.Write("Conversion Success (HonorBuddy Grind to Grinder bot): " + fileName);
                        return true;
                    }
                }
            }
            catch
            {
            }
            Logging.Write("Conversion Failled (HonorBuddy Grind to Grinder bot): " + path);
            return false;
        }
    }
}