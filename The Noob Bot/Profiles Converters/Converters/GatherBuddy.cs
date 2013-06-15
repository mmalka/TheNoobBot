using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Gatherer;
using nManager.Helpful;
using nManager.Wow.Class;

namespace Profiles_Converters.Converters
{
    public class GatherBuddy
    {
        public static bool IsGatherBuddyProfile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    var text = Others.ReadFile(path);
                    if (text.Contains("<HBProfile") || text.Contains("<GlideProfile"))
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
                if (IsGatherBuddyProfile(path))
                {
                    var _profile = new GathererProfile();

                    #region LoadProfileBuddy

                    var xml = XElement.Load(path);

                    // Loop through Elements Collection
                    foreach (XElement child in xml.Elements())
                    {
                        // Si glider profile
                        if (child.Name.ToString().ToLower() == "Waypoint".ToLower())
                        {
                            string tempsPosition = child.Value;

                            if (tempsPosition.Replace(" ", "").Length > 0)
                            {
                                var positionTempsString =
                                    tempsPosition.Replace("  ", " ").Split(System.Convert.ToChar(" "));
                                if (positionTempsString.Length == 3)
                                {
                                    try
                                    {
                                        _profile.Points.Add(new Point(
                                                                Others.ToSingle(
                                                                    positionTempsString[0]),
                                                                Others.ToSingle(
                                                                    positionTempsString[1]),
                                                                Others.ToSingle(
                                                                    positionTempsString[2]),
                                                                "Flying"));
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }

                        // Vendeur gather buddy
                        /*
                        if (child.Name.ToString().ToLower() == "Vendors".ToLower())
                        {
                            foreach (XElement childVendors in child.Elements())
                            {
                                try
                                {
                                    if (childVendors.Name.ToString().ToLower() == "Vendor".ToLower())
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
                                                    var npc =
                                                        new Npc
                                                            {
                                                                Position =
                                                                    new Point(
                                                                    Others.ToSingle(x.Value),
                                                                    Others.ToSingle(y.Value),
                                                                    Others.ToSingle(z.Value),
                                                                    "Flying"),
                                                                Entry =
                                                                    System.Convert.ToInt32(childVendors.Attribute("Entry")),
                                                                Faction = Npc.FactionType.Neutral,
                                                                Name = childVendors.Attribute("Name").ToString(),
                                                                Type =
                                                                    (Npc.NpcType)
                                                                    Enum.Parse(typeof (Npc.NpcType),
                                                                               childVendors.Attribute("Type").ToString(),
                                                                               true),
                                                                               ContinentId = ContinentId.None
                                                            }

                                                        ;
                                                    _profile.Npc.Add(npc);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch {}
                            }
                        }

                        */
                        // Position gather buddy
                        if (child.Name.ToString().ToLower() == "Hotspots".ToLower())
                        {
                            foreach (XElement childHotspots in child.Elements())
                            {
                                if (childHotspots.Name.ToString().ToLower() == "Hotspot".ToLower())
                                {
                                    XAttribute x = childHotspots.Attribute("X");
                                    if (x == null)
                                        x = childHotspots.Attribute("x");
                                    XAttribute y = childHotspots.Attribute("Y");
                                    if (y == null)
                                        y = childHotspots.Attribute("y");
                                    XAttribute z = childHotspots.Attribute("Z");
                                    if (z == null)
                                        z = childHotspots.Attribute("z");
                                    float xF;
                                    float yF;
                                    float zF;
                                    if (float.TryParse(x.Value, NumberStyles.Number, CultureInfo.InvariantCulture,
                                                       out xF))
                                    {
                                        if (float.TryParse(y.Value, NumberStyles.Number, CultureInfo.InvariantCulture,
                                                           out yF))
                                        {
                                            if (float.TryParse(z.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out zF))
                                            {
                                                var pT = new Point(xF, yF, zF, "Flying");
                                                _profile.Points.Add(pT);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion LoadProfileBuddy

                    var fileName = Path.GetFileNameWithoutExtension(path);
                    if (XmlSerializer.Serialize(Application.StartupPath + "\\Profiles\\Gatherer\\" + fileName + ".xml",
                                                _profile))
                    {
                        Logging.Write("Conversion Success (GatherBuddy to Gatherer bot): " + fileName);
                        return true;
                    }
                }
            }
            catch
            {
            }
            Logging.Write("Conversion Failled (GatherBuddy to Gatherer bot): " + path);
            return false;
        }
    }
}