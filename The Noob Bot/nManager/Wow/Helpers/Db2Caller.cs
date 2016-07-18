using System;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Patchables;
using SlimDX.Direct3D9;

namespace nManager.Wow.Helpers
{
    public class Db2Caller
    {
        public static uint WowClientDB2__GetRowPointer(int index, uint offset, bool box = false)
        {
            try
            {
                Usefuls.UpdateLastHardwareAction();

                var asm = new string[]
                {
                    "push 0",
                    "push 0",
                    "push 0",
                    "push " + (Memory.WowProcess.WowModule + (uint) Addresses.DBC.Unknown), //dunno what it is
                    "push " + index,
                    "mov ecx, " + (Memory.WowProcess.WowModule + offset), //db2pointer
                    "call " + (Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.WowClientDB2__GetRowPointer), //function
                    "retn"
                };

                //return Core.Memory.Read<IntPtr>(Core.Executor.Execute(asm, "DB2Reader"));
                    ;
                uint ret = Memory.WowMemory.InjectAndExecute(asm);
                if (box)
                    MessageBox.Show(ret.ToString("X"));
                return ret;
            }
            catch (Exception exception)
            {
                Logging.WriteError("WowClientDB2__GetRowPointer(int index, uint offset): " + exception);
            }
            return 0;
        }
    }
}