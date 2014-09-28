using System;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class ClickToMove
    {
        private static Vector3 cache = new Vector3();

        public static void CGPlayer_C__ClickToMove(Single x, Single y, Single z, UInt128 guid, Int32 action,
            Single precision)
        {
            try
            {
                Usefuls.UpdateLastHardwareAction();
                if (x == 0 && y == 0 && z == 0 && guid == 0)
                    return;

                // Allocate Memory:
                uint posCodecave = Memory.WowMemory.Memory.AllocateMemory(0x4*3);
                uint guidCodecave = Memory.WowMemory.Memory.AllocateMemory(0x8);
                uint precisionCodecave = Memory.WowMemory.Memory.AllocateMemory(0x4);
                if (posCodecave <= 0 || guidCodecave <= 0 || precisionCodecave <= 0)
                    return;
                // Write value:
                Memory.WowMemory.Memory.WriteInt128(guidCodecave, guid);
                Memory.WowMemory.Memory.WriteFloat(precisionCodecave, precision);

                Memory.WowMemory.Memory.WriteFloat(posCodecave, x);
                Memory.WowMemory.Memory.WriteFloat(posCodecave + 0x4, y);
                Memory.WowMemory.Memory.WriteFloat(posCodecave + 0x8, z);

                // BOOL __thiscall CGPlayer_C__ClickToMove(WoWActivePlayer *this, CLICKTOMOVETYPE clickType, WGUID *interactGuid, WOWPOS *clickPos, float precision)
                string[] asm = new[]
                {
                    "call " +
                    (Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayer)
                    ,
                    "test eax, eax",
                    "je @out",
                    "call " +
                    (Memory.WowProcess.WowModule +
                     (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayerObj),
                    "test eax, eax",
                    "je @out",
                    "mov edx, [" + precisionCodecave + "]",
                    "push edx",
                    "push " + posCodecave,
                    "push " + guidCodecave,
                    "push " + action,
                    "mov ecx, eax",
                    "call " +
                    (Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.CGUnit_C__InitializeTrackingState),
                    "@out:",
                    "retn"
                };

                Memory.WowMemory.InjectAndExecute(asm);
                Memory.WowMemory.Memory.FreeMemory(posCodecave);
                Memory.WowMemory.Memory.FreeMemory(guidCodecave);
                Memory.WowMemory.Memory.FreeMemory(precisionCodecave);
                if (cache != new Vector3(x, y, z))
                {
                    Logging.WriteNavigator("MoveTo(" + x + ", " + y + ", " + z + ", " + guid + ", " + action + ", " + precision + ")");
                    cache = new Vector3(x, y, z);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError(
                    "CGPlayer_C__ClickToMove(Single x, Single y, Single z, UInt64 guid, Int32 action, Single precision): " +
                    exception);
            }
        }

        public static Enums.ClickToMoveType GetClickToMoveTypePush()
        {
            try
            {
                return
                    (Enums.ClickToMoveType)
                        Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) Addresses.ClickToMove.CTM_PUSH);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetClickToMoveTypePush(): " + exception);
                return Enums.ClickToMoveType.None;
            }
        }

        public static Point GetClickToMovePosition()
        {
            try
            {
                return
                    (new Point(
                        Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule +
                                                          (uint) Addresses.ClickToMove.CTM_X),
                        Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule +
                                                          (uint) Addresses.ClickToMove.CTM_Y),
                        Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule +
                                                          (uint) Addresses.ClickToMove.CTM_Z)));
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetClickToMovePosition(): " + exception);
                return new Point(0, 0, 0);
            }
        }
    }
}