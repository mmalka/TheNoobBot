using nManager.Helpful;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class CGUnit_C__GetFacing
    {
        public static float GetFacing(uint baseAddress)
        {
            try
            {
                if (baseAddress > 0)
                {
                    uint VTable =
                        Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(baseAddress) +
                                                         ((uint) Addresses.VMT.CGUnit_C__GetFacing*4));
                    if (VTable <= 0)
                        return 0;
                    Addresses.ObjectManager objectManagerBase = Addresses.ObjectManager.objectManager;
                    if (objectManagerBase <= 0)
                        return 0;

                    uint result_Codecave = Memory.WowMemory.Memory.AllocateMemory(0x4 + 1 + Others.Random(1, 25));
                    if (result_Codecave <= 0)
                        return 0;
                    Memory.WowMemory.Memory.WriteFloat(result_Codecave, 0);

                    string[] asm = new[]
                        {
                            "call " +
                            (Memory.WowProcess.WowModule +
                             (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayer),
                            "test eax, eax",
                            "je @out",
                            "call " +
                            (Memory.WowProcess.WowModule +
                             (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayerObj),
                            "test eax, eax",
                            "je @out",
                            "mov ecx, " + baseAddress,
                            "call " + VTable,
                            "mov eax, " + result_Codecave,
                            "fstp dword [eax]",
                            "@out:",
                            "retn"
                        };


                    Memory.WowMemory.InjectAndExecute(asm);
                    float result = Memory.WowMemory.Memory.ReadFloat(result_Codecave);
                    Memory.WowMemory.Memory.FreeMemory(result_Codecave);

                    if (result < 0)
                        result = result + (float) System.Math.PI*2;

                    if (result > (float) System.Math.PI*2)
                        result = 0;
                    if (result < 0)
                        result = 0;

                    return result;
                }
            }
            catch
            {
            }
            return 0;
        }
    }
}