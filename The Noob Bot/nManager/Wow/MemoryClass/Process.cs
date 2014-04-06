using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using nManager.Helpful;
using nManager.Helpful.Win32;

namespace nManager.Wow.MemoryClass
{
    /// <summary>
    /// Process Manager
    /// </summary>
    public class Process
    {
        public Process()
        {
            ProcessId = 0;
        }

        public Process(int processId)
        {
            ProcessId = processId;
            OpenProcess();
        }

        /// <summary>
        /// Gets the Wow.exe module address.
        /// </summary>
        public uint WowModule { get; internal set; }

        /// <summary>
        /// Gets or sets the process handle.
        /// </summary>
        /// <value>
        /// The process handle.
        /// </value>
        public IntPtr ProcessHandle { get; set; }

        /// <summary>
        /// Gets or sets the main window handle.
        /// </summary>
        /// <value>
        /// The main window handle.
        /// </value>
        public IntPtr MainWindowHandle { get; internal set; }

        private int _processId = 0;

        /// <summary>
        /// Gets or sets the process id.
        /// </summary>
        /// <value>
        /// The process id.
        /// </value>
        public int ProcessId
        {
            get { return _processId; }
            set { _processId = value; }
        }

        /// <summary>
        /// Return a list of process.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param name="processName"></param>
        /// <returns name="processHandle"></returns>
        public static System.Diagnostics.Process[] ListeProcessIdByName(string processName = "pandashan.dat")
        {
            try
            {
                System.Diagnostics.Process[] processesByNameList =
                    System.Diagnostics.Process.GetProcessesByName(processName);
                return processesByNameList;
            }
            catch (Exception e)
            {
                Logging.WriteError("ListeProcessIdByName(string processName = \"Wow\"): " + e);
            }
            return new System.Diagnostics.Process[0];
        }

        /// <summary>
        /// Return true if process exist.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <returns name="processHandle"></returns>
        public bool ProcessExist()
        {
            try
            {
                return System.Diagnostics.Process.GetProcessById(ProcessId).Id == ProcessId;
            }
            catch (Exception e)
            {
                Logging.WriteError("ProcessExist(): " + e);
                return false;
            }
        }

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns></returns>
        public UInt32 GetModule(string moduleName)
        {
            try
            {
                ProcessModuleCollection modules = System.Diagnostics.Process.GetProcessById(ProcessId).Modules;
                for (int i = 0; i < modules.Count; i++)
                {
                    if (modules[i].ModuleName.ToLower() == moduleName.ToLower())
                    {
                        return (uint) modules[i].BaseAddress;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("GetModule(string moduleName): " + e);
            }
            return 0;
        }

        /// <summary>
        /// Open process on all access mode and enter on debug mode.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <returns name="processHandle"></returns>
        public IntPtr OpenProcess()
        {
            try
            {
                System.Diagnostics.Process.EnterDebugMode();

                ProcessHandle = Native.OpenProcess(0x1F0FFF, false, ProcessId);

                System.Diagnostics.Process processById = System.Diagnostics.Process.GetProcessById(ProcessId);
                MainWindowHandle = processById.MainWindowHandle;
                WowModule = GetModule("pandashan.dat");
                return ProcessHandle;
            }
            catch (Exception e)
            {
                Logging.WriteError("OpenProcess(): " + e);
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// Close all access mode of process and close debug mode.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <returns></returns>
        public void CloseProcessHandle()
        {
            try
            {
                Native.CloseHandle(ProcessHandle);
                System.Diagnostics.Process.LeaveDebugMode();
                ProcessHandle = IntPtr.Zero;
                MainWindowHandle = IntPtr.Zero;
                ProcessId = (int) IntPtr.Zero;
            }
            catch (Exception e)
            {
                Logging.WriteError("CloseProcessHandle(): " + e);
            }
        }

        /// <summary>
        /// Kill and close Wow process.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <returns></returns>
        public void KillWowProcess()
        {
            try
            {
                System.Diagnostics.Process.GetProcessById(ProcessId).Kill();
            }
            catch (Exception e)
            {
                Logging.WriteError("KillWowProcess(): " + e);
            }
        }


        internal static List<int> InjectionCount = new List<int>();

        public static int GetInjectionBySec()
        {
            try
            {
                for (int i = InjectionCount.Count - 1; i >= 0; i--)
                {
                    if (InjectionCount[i] < Others.Times - (1000*1))
                        InjectionCount.RemoveAt(i);
                }
                return InjectionCount.Count;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetInjectionByMin(): " + e);
                return 0;
            }
        }
    }
}