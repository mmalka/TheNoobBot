using System;
using System.Collections.Generic;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class EventsListener
    {
        public delegate void CallBack(object context);

        private static List<HookedEventInfo> _hookedEvents = new List<HookedEventInfo>();
        private static Thread _threadHookEvent = null;
        private static readonly uint PtrFirstEvent = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.EventsListener.BaseEvents);
        private static readonly int EventsCount = Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) Addresses.EventsListener.EventsCount);

        /*private static void EnumWoWEventsDumper()
        {
            string eventsList = "";
            for (uint i = 0; i <= EventsCount-1; i++)
            {
                uint ptrCurrentEvent = Memory.WowMemory.Memory.ReadUInt(PtrFirstEvent + 0x4*i);
                if (ptrCurrentEvent > 0)
                {
                    uint ptrCurrentEventName = Memory.WowMemory.Memory.ReadUInt(ptrCurrentEvent + (uint) Addresses.EventsListener.EventOffsetName);
                    if (ptrCurrentEventName <= 0) continue;
                    string currentEventName = Memory.WowMemory.Memory.ReadASCIIString(ptrCurrentEventName);
                    if (currentEventName == "") continue;
                    if (eventsList.Contains(Environment.NewLine + currentEventName + " = ")) continue;
                    object previousLine = eventsList;
                    eventsList = string.Concat(new[] {previousLine, currentEventName, " = ", i, ",", Environment.NewLine});
                }
            }
            Logging.Write(eventsList);
        }*/

        private static int GetEventFireCount(WoWEventsType eventType)
        {
            uint eventId = (uint) eventType;
            if (eventId > EventsCount)
                return 0;
            uint ptrCurrentEvent = Memory.WowMemory.Memory.ReadUInt(PtrFirstEvent + 4*eventId);
            if (ptrCurrentEvent <= 0) return 0;
            uint currentEventNamePtr = Memory.WowMemory.Memory.ReadUInt(ptrCurrentEvent + (uint) Addresses.EventsListener.EventOffsetName);
            return currentEventNamePtr > 0 ? Memory.WowMemory.Memory.ReadInt(ptrCurrentEvent + (uint)Addresses.EventsListener.EventOffsetCount) : 0;
        }

        private static bool IsAttached(WoWEventsType eventType)
        {
            try
            {
                foreach (HookedEventInfo c in _hookedEvents)
                {
                    if (c.EventType == eventType) return true;
                }
                return false;
            }
            catch (Exception arg)
            {
                Logging.WriteError("IsAttached(string id, MethodDelegate method): " + arg);
            }
            return false;
        }

        public static void HookEvent(WoWEventsType eventType, CallBack method, bool forceHook = false)
        {
            try
            {
                lock ("LockEvent")
                {
                    if (IsAttached(eventType) && !forceHook)
                    {
                        Logging.WriteError("The event " + eventType.ToString() + " is already hooked and parameter forceHook is passed with false.");
                        return;
                    }
                    _hookedEvents.Add(new HookedEventInfo(method, eventType, GetEventFireCount(eventType)));
                    if (_threadHookEvent == null)
                        _threadHookEvent = new Thread(Hook) { Name = "Hook of Events" };
                    if (!_threadHookEvent.IsAlive)
                        _threadHookEvent.Start();
                }
            }
            catch (Exception arg)
            {
                Logging.WriteError("HookEvent(WoWEventsType eventType, CallBack method, bool forceHook = false): " + arg);
            }
        }

        public static void UnHookEvent(WoWEventsType eventType)
        {
            try
            {
                lock ("LockEvent")
                {
                    HookedEventInfo toRemove = null;
                    foreach (HookedEventInfo current in _hookedEvents)
                    {
                        if (current.EventType == eventType)
                        {
                            toRemove = current;
                            break;
                        }
                    }
                    if (toRemove != null)
                        _hookedEvents.Remove(toRemove);
                }
            }
            catch (Exception err)
            {
                Logging.WriteError("UnHookEvent(WoWEventsType eventType): " + err);
            }
        }

        private static void Hook()
        {
            while (_hookedEvents.Count > 0)
            {
                lock ("LockEvent")
                {
                    foreach (HookedEventInfo current in _hookedEvents)
                    {
                        if (current.PreviousCurrentEventFireCount >= GetEventFireCount(current.EventType)) continue;
                        Thread thread = new Thread(eventType => current.CallBack(current.EventType)) { Name = "Fire callback for Event: " + current.EventType };
                        thread.Start();
                        current.PreviousCurrentEventFireCount++;
                    }
                }
                Thread.Sleep(100);
            }
        }

        private class HookedEventInfo
        {
            internal readonly WoWEventsType EventType;
            private readonly CallBack _callBack;
            internal int PreviousCurrentEventFireCount = -1;

            internal HookedEventInfo(CallBack callBack, WoWEventsType id, int idUsedLastCount)
            {
                _callBack = callBack;
                EventType = id;
                PreviousCurrentEventFireCount = idUsedLastCount;
            }

            internal void CallBack(WoWEventsType eventType)
            {
                switch (eventType)
                {
                    case WoWEventsType.CHAT_MSG_LOOT:
                        // when looting multiple items, fire multiples times, we want to make sure to jump to the latest eventFireCount.
                        Thread.Sleep(500); // Allow some times to the bot to mount up etc before slowing down because of the ObjectList stuff.
                        _callBack(GetEventFireCount(eventType));
                        break;
                    default:
                        _callBack(null);
                        break;
                }
            }
        }
    }
}