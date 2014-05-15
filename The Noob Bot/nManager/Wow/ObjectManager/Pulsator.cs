using System;
using System.Collections.Generic;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Helpers;
using System.Collections.Concurrent;

namespace nManager.Wow.ObjectManager
{
    public static class Pulsator
    {
        private static Thread _worker;

        /// <summary>
        /// Initializes the pulsator with the specified process ID, optionally creating a thread
        /// that runs at roughly 30FPS to pulse the object manager.
        /// </summary>
        /// <param name="useThread"></param>
        public static void Initialize(bool useThread = true)
        {
            try
            {
                if (useThread)
                {
                    if (_worker != null)
                        Shutdown();
                    _worker = new Thread(Pulse) {IsBackground = true, Name = "ObjectManager"};
                    _worker.Start();
                }
                else
                {
                    Pulse();
                }
                Thread.Sleep(300);
            }
            catch (Exception e)
            {
                Logging.WriteError("Pulsator > Initialize(bool useThread = true): " + e);
            }
        }

        /// <summary>
        /// Pulses the object manager. This should be called from an EndScene hook, or some other form of 'OnFrame'
        /// type function. (This class has a built in thread handler to deal with said OnFrame function)
        /// </summary>
        public static void Pulse()
        {
            try
            {
                if (_worker == null)
                {
                    ObjectManager.Pulse();
                }
                else
                {
                    while (true)
                    {
                        if (Memory.WowMemory.ThreadHooked && Memory.WowMemory.Memory.IsProcessOpen && Usefuls.InGame)
                        {
                            ObjectManager.Pulse();
                            if (Usefuls.InGame && !ObjectManager.Me.IsValid)
                            {
                                nManager.Pulsator.Reset();
                            }
                        }
                        else
                        {
                            ObjectManager.ObjectDictionary = new ConcurrentDictionary<ulong, WoWObject>();
                        }
                        Thread.Sleep(650);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Pulsator > Pulse(): " + e);
                _worker = null;
            }
        }

        /// <summary>
        /// Shuts down the pulsator, and closes the opened process handle.
        /// </summary>
        public static void Shutdown()
        {
            try
            {
                if (_worker != null)
                {
                    _worker.Abort();
                    while (_worker != null && _worker.IsAlive) // || _worker.ThreadState != ThreadState.Aborted)
                    {
                        // Wait for the thread to actually die.
                        // This avoids crashing WoW at shutdown.
                    }
                    _worker = null;
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Pulsator > Shutdown(): " + e);
                _worker = null;
            }
        }

        /// <summary>
        /// ObjectManager Active.
        /// </summary>
        public static bool IsActive
        {
            get
            {
                try
                {
                    return _worker != null;
                }
                catch (Exception e)
                {
                    Logging.WriteError("Pulsator > IsActive: " + e);
                }
                return false;
            }
        }
    }
}