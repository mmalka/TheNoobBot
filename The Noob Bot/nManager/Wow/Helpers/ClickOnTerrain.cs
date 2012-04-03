using System.Runtime.InteropServices;
using System.Threading;
using nManager.Wow.Class;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class ClickOnTerrain
    {
        #region Strut

        [StructLayout(LayoutKind.Explicit, Size = 0x20)]
        public struct StructClickOnTerrain
        {
            [FieldOffset(0x0)]
            public ulong Guid;
            [FieldOffset(0x8)]
            public float X;
            [FieldOffset(0xC)]
            public float Y;
            [FieldOffset(0x10)]
            public float Z;
        }

        #endregion Strut

        public static void Spell(uint spellId, Point point)
        {
            if (spellId <= 0)
                return;
            if (point == null)
                return;
            if (point.X == 0 && point.Y == 0)
                return;

            var s = new Spell(spellId);

            s.Launch();

            Thread.Sleep(Usefuls.Latency + 50);

            Pulse(point);
        }

        public static void Item(uint itemId, Point point)
        {
            if (itemId <= 0)
                return;
            if (point == null)
                return;
            if (point.X == 0 && point.Y == 0)
                return;

            ItemsManager.UseItem(ItemsManager.GetNameById(itemId));


            Thread.Sleep(Usefuls.Latency + 50);

            Pulse(point);
        }

        static void Pulse(Point point)
        {
            try
            {
                if (point == null)
                    return;
                if (point.X == 0 && point.Y == 0)
                    return;

                var codeCaveStructClickOnTerrain = Memory.WowMemory.Memory.AllocateMemory(Marshal.SizeOf(typeof(StructClickOnTerrain)));

                //Struct
                var structClickOnTerrain = new StructClickOnTerrain {X = point.X, Y = point.Y, Z = point.Z};

                // WRITE
                Memory.WowMemory.Memory.WriteObject(codeCaveStructClickOnTerrain, structClickOnTerrain, typeof(StructClickOnTerrain));


                var asm = new[]
                {
                "call " + (Memory.WowProcess.WowModule + (uint)Addresses.FunctionWow.ClntObjMgrGetActivePlayer),
                "test eax, eax",
                "je @out",

                "call " + (Memory.WowProcess.WowModule + (uint)Addresses.FunctionWow.ClntObjMgrGetActivePlayerObj),
                "test eax, eax",
                "je @out",

                "push " + codeCaveStructClickOnTerrain,
                "mov ebx, " + (Memory.WowProcess.WowModule + (uint)Addresses.FunctionWow.Spell_C__HandleTerrainClick),
                "call ebx",
                "add esp, 0x4",

                "@out:",
                "retn"
                 };

                Memory.WowMemory.InjectAndExecute(asm);

                Memory.WowMemory.Memory.FreeMemory(codeCaveStructClickOnTerrain);
            }
            catch { }
        }
    }
}
