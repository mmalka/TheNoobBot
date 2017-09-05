// Copyright (C) 2017 Julian Bosch
// See the file LICENSE for copying permission.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using MyMemory;
using MyMemory.Memory;
using nManager.Helpful;

namespace nManager.Wow.MemoryClass
{
    /// <summary>
    /// Class that allow calling code by the message handler thread in a remote process
    /// </summary>
    public class WndProcExecutor2 : IDisposable
    {
        private const int GWL_WNDPROC = -4;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr p_Hwnd, uint p_Msg, IntPtr p_WParam, IntPtr p_LParam);

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress(IntPtr p_Module, string p_ProcName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetModuleHandle(string p_ModuleName);

        private readonly RemoteProcess m_Process;
        private readonly IntPtr m_WindowHandle;
        private readonly Random m_Random;
        private readonly int m_CustomMessageCode;
        private readonly RemoteAllocatedMemory m_Data;
        private readonly IntPtr m_WndProcFunction;
        private readonly IntPtr m_OriginalWndProc;
        private Thread wndProcThread;

        public void WndProcChecker()
        {
            while (true)
            {
                if (m_WindowHandle != Memory.WowProcess.MainWindowHandle)
                {
                    if (Memory.WowProcess != null && Memory.WowProcess.ProcessExist())
                    {
                        if (Memory.WowProcess.Executor != null)
                            Memory.WowProcess.Executor.Dispose();
                        var process = new RemoteProcess((uint) Memory.WowProcess.ProcessId);
                        Memory.WowProcess.Executor = new WndProcExecutor2(process, Memory.WowProcess.MainWindowHandle);
                    }
                    break;
                }
                Thread.Sleep(5000);
            }
        }

        public WndProcExecutor2(RemoteProcess p_Process, IntPtr p_Windowhandle)
        {
            m_WindowHandle = p_Windowhandle;
            m_Process = p_Process;
            m_Random = new Random();
            m_CustomMessageCode = m_Random.Next(0x8000, 0xBFFF); // From MSDN : WM_APP (0x8000) through 0xBFFF
            m_Data = m_Process.MemoryManager.AllocateMemory(0x1000);
            m_WndProcFunction = m_Data.AllocateOfChunk("WndProc", 0x500);
            m_OriginalWndProc = m_Data.AllocateOfChunk<IntPtr>("OriginalWndProc");

            IntPtr l_User32 = GetModuleHandle("user32.dll");
            IntPtr l_CallWindowProcW = GetProcAddress(l_User32, "CallWindowProcW");
            IntPtr l_SetWindowLongW = GetProcAddress(l_User32, "SetWindowLongW");

            // Setup our WndProc callback
            var l_Mnemonics = new[]
            {
                "mov eax, [esp+0x8]", // Get the message code from the stack
                "cmp eax, " + m_CustomMessageCode + "", // Check if the message code is our custom one
                "jne @call_original", // Otherwise simply call the original WndProc
                
                "mov eax, [esp+0xC]", // Function pointer
                "mov edx, [esp+0x10]", // Result pointer
                "push edx", // Save result pointer
                "call eax", // Call the user function
                "pop edx", // Restore the result pointer
                "mov [edx], eax", // Save user function result
                "xor eax, eax", // We handled the message
                "retn",
                "@call_original:",
                "mov ecx, [esp+0x4]", // Hwnd
                "mov edx, [esp+0x8]", // Msg
                "mov esi, [esp+0xC]", // WParam
                "mov edi, [esp+0x10]", // LParam
                "mov eax, [" + m_OriginalWndProc + "]",
                "push edi", // LParam
                "push esi", // WParam
                "push edx", // Msg
                "push ecx", // Hwnd
                "push eax", // WndProc original
                "call " + l_CallWindowProcW + "", // Call the original WndProc
                "retn 0x14",
            };
            m_Process.Yasm.Inject(l_Mnemonics, m_WndProcFunction);

            // Register our WndProc callback
            l_Mnemonics = new[]
            {
                "push " + m_WndProcFunction + "",
                "push " + GWL_WNDPROC + "",
                "push " + m_WindowHandle + "",
                "call " + l_SetWindowLongW + "",
                "mov [" + m_OriginalWndProc + "], eax",
                "retn"
            };


            var m_Data2 = m_Process.MemoryManager.AllocateMemory(0x1000);
            m_Process.Yasm.InjectAndExecute(l_Mnemonics, m_Data2.Pointer);

            wndProcThread = new Thread(WndProcChecker);
            wndProcThread.Start();
            //m_Process.Yasm.InjectAndExecute(l_Mnemonics);
        }

        public void Dispose()
        {
            IntPtr l_User32 = GetModuleHandle("user32.dll");
            IntPtr l_SetWindowLongW = GetProcAddress(l_User32,
                "SetWindowLongW");

            // Restore the original WndProc callback
            var l_Mnemonics = new[]
            {
                "mov eax, [" + m_OriginalWndProc + "]",
                "push eax",
                "push " + GWL_WNDPROC + "",
                "push " + m_WindowHandle + "",
                "call " + l_SetWindowLongW + "",
                "retn"
            };
            m_Process.Yasm.InjectAndExecute(l_Mnemonics);

            if (m_Data != null)
                m_Data.Dispose();
        }

        /// <summary>
        /// Call the specifics asm mnemonics in the message handler thread
        /// </summary>
        /// <param name="p_Mnemonics"></param>
        /// <param name="p_BufferSize"></param>
        /// <returns></returns>
        public IntPtr Call(string[] p_Mnemonics, uint p_BufferSize = 0x1000)
        {
            using (var l_Buffer = m_Process.MemoryManager.AllocateMemory(p_BufferSize))
            {
                m_Process.Yasm.Inject(p_Mnemonics, l_Buffer.Pointer);

                return Call(l_Buffer.Pointer);
            }
        }

        /// <summary>
        /// Call the specific function in the message handler thread
        /// The function pointer is passed in WParam
        /// The result pointer is passed in LParam
        /// </summary>
        /// <param name="p_Function">The function pointer to call</param>
        /// <returns>The value of EAX after calling the function</returns>
        public IntPtr Call(IntPtr p_Function)
        {
            using (var l_ResultBuffer = m_Process.MemoryManager.AllocateMemory((uint) IntPtr.Size))
            {
                SendMessage(m_WindowHandle, (uint) m_CustomMessageCode, p_Function, l_ResultBuffer.Pointer);

                //Console.WriteLine(Hook.CurrentCallStack);
                return l_ResultBuffer.Read<IntPtr>();
            }
        }
    }
}