using System;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class TraceLine
    {
        private static Point _toLast = new Point();
        private static Point _fromLast = new Point();
        private static bool _lastResult = true;

        public static bool TraceLineGo(Point to)
        {
            try
            {
                return TraceLineGo(ObjectManager.ObjectManager.Me.Position, to);
            }
            catch (Exception exception)
            {
                Logging.WriteError("TraceLineGo(Point to): " + exception);
                return true;
            }
        }

        public static bool TraceLineGo(Point from, Point to,
                                       Enums.CGWorldFrameHitFlags hitFlags = Enums.CGWorldFrameHitFlags.HitTestAll)
        {
            try
            {
                if (from.X != 0 && from.Y != 0 && to.X != 0 && to.Y != 0)
                {
                    // cache:
                    if (_toLast.DistanceTo(to) < 1.5f && _fromLast.DistanceTo(from) < 1.5f)
                    {
                        return _lastResult;
                    }
                    _toLast = to;
                    _fromLast = from;

                    // protected delegate byte Traceline(ref WoWPoint end, ref WoWPoint start, ref WoWPoint result, ref float distance, uint flags, ref WoWPoint Optional);
                    var end = Memory.WowMemory.Memory.AllocateMemory(0x4*3);
                    var start = Memory.WowMemory.Memory.AllocateMemory(0x4*3);
                    var result = Memory.WowMemory.Memory.AllocateMemory(0x4*3);
                    var distance = Memory.WowMemory.Memory.AllocateMemory(0x4);
                    var optional = Memory.WowMemory.Memory.AllocateMemory(0x4*3);
                    var resultRet = Memory.WowMemory.Memory.AllocateMemory(0x4);


                    if (end <= 0 || start <= 0 || result <= 0 || distance <= 0 || optional <= 0)
                        return false;

                    Memory.WowMemory.Memory.WriteFloat(optional, 0);
                    Memory.WowMemory.Memory.WriteFloat(optional + 0x4, 0);
                    Memory.WowMemory.Memory.WriteFloat(optional + 0x8, 0);

                    Memory.WowMemory.Memory.WriteFloat(distance, 0.9f);

                    Memory.WowMemory.Memory.WriteFloat(result, 0);
                    Memory.WowMemory.Memory.WriteFloat(result + 0x4, 0);
                    Memory.WowMemory.Memory.WriteFloat(result + 0x8, 0);

                    Memory.WowMemory.Memory.WriteFloat(start, from.X);
                    Memory.WowMemory.Memory.WriteFloat(start + 0x4, from.Y);
                    Memory.WowMemory.Memory.WriteFloat(start + 0x8, from.Z + 1.5f);

                    Memory.WowMemory.Memory.WriteFloat(end, to.X);
                    Memory.WowMemory.Memory.WriteFloat(end + 0x4, to.Y);
                    Memory.WowMemory.Memory.WriteFloat(end + 0x8, to.Z + 1.5f);
                    Memory.WowMemory.Memory.WriteInt(resultRet, 0);

                    var asm = new[]
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
                            "push " + 0,
                            "push " + (uint) hitFlags,
                            "push " + distance,
                            "push " + result,
                            "push " + start,
                            "push " + end,
                            "call " +
                            (Memory.WowProcess.WowModule +
                             (uint) Addresses.FunctionWow.CGWorldFrame__Intersect),
                            "mov [" + resultRet + "], al",
                            "add esp, " + (uint) 0x18,
                            "@out:",
                            "retn"
                        };


                    Memory.WowMemory.InjectAndExecute(asm);
                    var ret = Memory.WowMemory.Memory.ReadInt(resultRet) > 0;

                    Memory.WowMemory.Memory.FreeMemory(resultRet);
                    Memory.WowMemory.Memory.FreeMemory(end);
                    Memory.WowMemory.Memory.FreeMemory(start);
                    Memory.WowMemory.Memory.FreeMemory(result);
                    Memory.WowMemory.Memory.FreeMemory(distance);
                    Memory.WowMemory.Memory.FreeMemory(optional);

                    _lastResult = ret;
                    return ret;
                }
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError(
                    "TraceLineGo(Point from, Point to, Enums.CGWorldFrameHitFlags hitFlags = Enums.CGWorldFrameHitFlags.HitTestAll): " +
                    exception);
                return true;
            }
        }
    }
}