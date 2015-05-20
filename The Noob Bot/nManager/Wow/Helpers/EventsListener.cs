using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        private static Object _ourLock = new Object();

        private static void EnumWoWEventsDumper()
        {
            string eventsList = "";
            for (uint i = 0; i <= EventsCount - 1; i++)
            {
                uint ptrCurrentEvent = Memory.WowMemory.Memory.ReadUInt(PtrFirstEvent + 0x4*i);
                if (ptrCurrentEvent > 0)
                {
                    uint ptrCurrentEventName = Memory.WowMemory.Memory.ReadUInt(ptrCurrentEvent + (uint) Addresses.EventsListener.EventOffsetName);
                    if (ptrCurrentEventName <= 0) continue;
                    string currentEventName = Memory.WowMemory.Memory.ReadUTF8String((ptrCurrentEventName));
                    if (currentEventName == "") continue;
                    if (eventsList.Contains(Environment.NewLine + currentEventName + " = ")) continue;
                    object previousLine = eventsList;
                    eventsList = string.Concat(new[] {previousLine, currentEventName, " = ", i, ",", Environment.NewLine});
                }
            }
            Logging.Write(eventsList);
        }

        private static int GetEventFireCount(WoWEventsType eventType)
        {
            uint eventId = (uint) eventType;
            if (eventId > EventsCount)
                return 0;
            uint ptrCurrentEvent = Memory.WowMemory.Memory.ReadUInt(PtrFirstEvent + 4*eventId);
            if (ptrCurrentEvent <= 0) return 0;
            uint currentEventNamePtr = Memory.WowMemory.Memory.ReadUInt(ptrCurrentEvent + (uint) Addresses.EventsListener.EventOffsetName);
            return currentEventNamePtr > 0 ? Memory.WowMemory.Memory.ReadInt(ptrCurrentEvent + (uint) Addresses.EventsListener.EventOffsetCount) : 0;
        }

        private static bool IsAttached(WoWEventsType eventType, string callBack, bool sendsFireCount = false)
        {
            try
            {
                lock (_ourLock)
                {
                    foreach (HookedEventInfo c in _hookedEvents)
                    {
                        if (c.EventType == eventType && c.CallBackName == callBack && c.SendsFireCount == sendsFireCount) return true;
                    }
                }
                return false;
            }
            catch (Exception arg)
            {
                Logging.WriteError("IsAttached(string id, MethodDelegate method): " + arg);
            }
            return false;
        }

        public static void HookEvent(WoWEventsType eventType, Expression<CallBack> method, bool requestFireCount = false, bool ignoreAlreadyDone = false)
        {
            try
            {
                Logging.WriteDebug("Init HookEvent for event: " + eventType + " with CallBack: " + method + " and requestFireCount: " + requestFireCount);
                if (IsAttached(eventType, method.ToString(), requestFireCount))
                {
                    if (!ignoreAlreadyDone)
                        Logging.WriteError("The event " + eventType + " with method " + method + " and parameter requestFireCount set to " + requestFireCount +
                                       " is already hooked in the exact same way, duplicates of HookEvent is a bad code manner, make sure to UnHook your event when your Stop() your plugin.");
                    return;
                }
                lock (_ourLock)
                {
                    CallBack callBack = method.Compile();
                    _hookedEvents.Add(new HookedEventInfo(callBack, eventType, GetEventFireCount(eventType), method.ToString(), requestFireCount));
                }
                if (_threadHookEvent == null || !_threadHookEvent.IsAlive)
                {
                    _threadHookEvent = new Thread(Hook) {Name = "Hook of Events"};
                    _threadHookEvent.Start();
                }
            }
            catch (Exception arg)
            {
                Logging.WriteError("HookEvent(WoWEventsType eventType, Expression<CallBack> method, bool requestFireCount = false): " + arg);
            }
        }

        public static void UnHookEvent(WoWEventsType eventType, Expression<CallBack> method, bool requestFireCount = false)
        {
            try
            {
                Logging.WriteDebug("Init UnHookEvent for event: " + eventType + " with CallBack: " + method + " and requestFireCount: " + requestFireCount);
                lock (_ourLock)
                {
                    HookedEventInfo toRemove = null;
                    foreach (HookedEventInfo current in _hookedEvents)
                    {
                        if (current.EventType == eventType && current.CallBackName == method.ToString() && current.SendsFireCount == requestFireCount)
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
                Logging.WriteError("UnHookEvent(WoWEventsType eventType, string methodName, bool requestFireCount = false): " + err);
            }
        }

        private static void Hook()
        {
            while (_hookedEvents.Count > 0)
            {
                lock (_ourLock)
                {
                    foreach (HookedEventInfo current in _hookedEvents)
                    {
                        if (current.PreviousCurrentEventFireCount >= GetEventFireCount(current.EventType)) continue;
                        Thread thread = new Thread(current.CallBack) {Name = "Fire callback for Event: " + current.EventType};
                        thread.Start();
                        current.PreviousCurrentEventFireCount++;
                    }
                }
                Thread.Sleep(100);
            }
        }

        private class HookedEventInfo
        {
            internal readonly string CallBackName;
            internal readonly WoWEventsType EventType;
            internal readonly bool SendsFireCount;
            private readonly CallBack _callBack;
            internal int PreviousCurrentEventFireCount = -1;

            internal HookedEventInfo(CallBack callBack, WoWEventsType id, int idUsedLastCount, string callBackName, bool sendsFireCount)
            {
                _callBack = callBack;
                EventType = id;
                PreviousCurrentEventFireCount = idUsedLastCount;
                CallBackName = callBackName;
                SendsFireCount = sendsFireCount;
            }

            internal void CallBack()
            {
                switch (EventType)
                {
                    case WoWEventsType.CHAT_MSG_LOOT:
                        // when looting multiple items, fire multiples times, we want to make sure to jump to the latest eventFireCount.
                        Thread.Sleep(2000); // Allow some times to the bot to mount up etc before slowing down because of the ObjectList stuff.
                        break;
                }
                if (SendsFireCount)
                    _callBack(GetEventFireCount(EventType));
                else
                    _callBack(null);
            }
        }
    }
}