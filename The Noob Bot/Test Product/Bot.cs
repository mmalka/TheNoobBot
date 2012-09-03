using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using nManager.Wow;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.Patchables;

namespace Test_Product
{
    class Bot
    {
        public static bool Pulse()
        {
            var idEquiped = nManager.Wow.ObjectManager.ObjectManager.Me.GetDescriptor<uint>(Descriptors.PlayerFields.visibleItems + 15 * 2);
            /*
            DBC<DBCStruct.SpellRec> DBCSpell = new DBC<DBCStruct.SpellRec>((int)Addresses.DBC.spell);
            var sw = new StreamWriter(Application.StartupPath + "\\spell.txt", true, Encoding.UTF8);
            for (int i = 0; i < DBCSpell.MaxIndex - 1; i++)
            {
                if (DBCSpell.HasRow(i))
                {
                    var t = DBCSpell.GetRow(i);

                    sw.Write(t.SpellId + ";" + Memory.WowMemory.Memory.ReadUTF8String(t.Name) + Environment.NewLine);
                }
            }
            sw.Close();

            //Cheat.AntiAfkPulse();
            /*
            //D3D9
            const int VMT_ENDSCENE = 42;
            using (var d3d = new Direct3D())
            {
                using (var tmpDevice = new Device(d3d, 0, DeviceType.Hardware, IntPtr.Zero, CreateFlags.HardwareVertexProcessing, new PresentParameters() { BackBufferWidth = 1, BackBufferHeight = 1 }))
                {
                    var EndScenePointer = nManager.Wow.Memory.WowMemory.Memory.ReadUInt(nManager.Wow.Memory.WowMemory.Memory.ReadUInt((uint)tmpDevice.ComPointer) + VMT_ENDSCENE * 4);
                }
            }
            
            return false;

            var t = new []
                        {
"###############################################################",
"#xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx#",
"##xxx##############################################x#########x#",
"####xx#############################################x#########x#",
"#####xx############################################x#########x#",
"######xx###########################################x###########",
"#######xx##########################################xxxxxxxxxxx#",
"########xx###################################################x#",
"#########xx##################################################x#",
"##########x##################################################x#",
"##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx#########x#",
"##x#########xx###############################################x#",
"##x##########xx##############################################x#",
"##x###########xx#############################################x#",
"##x############xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx#########x#",
"##x#############xx#############################################",
"##x##############xx############################################",
"##xxx#############xx###########################################",
"###################xx##########################################",
"####################xx#########################################",
"#####################xx########################################",
"##x#######xxxxxxxxxxxxxx#######################################",
"##x#######x############xx######################################",
"##x#####xxx#############xx#####################################",
"##x######################xx####################################",
"##xxxxxxxx#x###x##########xx###################################",
"##x######x#x###############xxxxxxxxxxxxxxxxxxxxxxxxx###########",
"##x######xxxx#x#####x#######xx#################################",
"##x########x#################xx################################",
"##x########x##################xxxxxxxx#########################",
"##x########x###################x###############################",
"#xxxxxxxxx#xxxxxxxxxxxxxxxxxxxxxx##############################",
"###############################################################",
                        }; // <> = X = c ; ^ = y = l

            var r = "";

            foreach (var lr in t)
            {
                r += lr + Environment.NewLine;
            }

            var listP = new List<Point>();

            for (int l = 0; l <= t.Length-1; l++)
            {
                for (int c = 0; c <= t[l].ToCharArray().Length-1; c++)
                {
                    if (t[l].ToCharArray()[c].ToString() == "x")
                        listP.Add(new Point(c, l, 0));
                }
            }
            var time = nManager.Helpful.Others.Times;
            var path = nManager.Wow.Helpers.PathFinder.FindPath(new Point(-10, 31, 0), new Point(61, 14, 0), listP, 10f, 10f);
            time = nManager.Helpful.Others.Times - time;
            var n = 0;
            foreach (var point in path)
            {
                var l = t[(int)point.Y].ToCharArray();
                l[(int)point.X] = Convert.ToChar(n.ToString());
                t[(int)point.Y] = new String(l);
                n++;
                if (n > 9)
                    n = 1;
            }

            r += Environment.NewLine + Environment.NewLine + "Path count: " + path.Count + " | " + time + " ms" +  Environment.NewLine;
            foreach (var lr in t)
            {
                r += lr + Environment.NewLine;
            }

            nManager.Helpful.Others.WriteFile("TESTTEST.txt", r);
            */
            return false;
        }

        public static void Dispose()
        {

        }
    }
}
