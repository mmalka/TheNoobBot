using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Fasm;
using MyMemory;
using nManager.Helpful;
using nManager.Wow.MemoryClass.Magic;
using nManager.Wow.Patchables;
using SlimDX.DirectWrite;

namespace nManager.Wow.MemoryClass
{
    /// <summary>
    /// Class that allow calling code by the message handler thread in a remote process
    /// </summary>
    public class WndProcExecutor : IDisposable
    {
        private const int GWL_WNDPROC = -4;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr p_Hwnd, uint p_Msg, IntPtr p_WParam, IntPtr p_LParam);

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress(IntPtr p_Module, string p_ProcName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetModuleHandle(string p_ModuleName);

        private readonly IntPtr m_WindowHandle;
        private readonly Random m_Random;
        private readonly int m_CustomMessageCode;
        private uint m_Injected;
        private readonly uint m_WndProcFunction;
        private readonly uint m_OriginalWndProc;

        public WndProcExecutor(BlackMagic memory)
        {
            try
            {
                m_WindowHandle = Memory.WowProcess.MainWindowHandle;
                m_Random = new Random();
                m_CustomMessageCode = m_Random.Next(0x8000, 0xBFFF); // From MSDN : WM_APP (0x8000) through 0xBFFF
                m_WndProcFunction = memory.AllocateMemory(0x500); // WndProc
                m_OriginalWndProc = memory.AllocateMemory(0x4); // WndProcOriginalWndProc

                IntPtr l_User32 = GetModuleHandle("user32.dll");
                IntPtr l_CallWindowProcW = GetProcAddress(l_User32, "CallWindowProcW");
                IntPtr l_SetWindowLongW = GetProcAddress(l_User32, "SetWindowLongW");


                var fasm = new ManagedFasm(Memory.WowProcess.ProcessHandle);

                fasm.SetMemorySize(0x1000);
                fasm.SetPassLimit(100);
                fasm.AddLine("mov eax, [esp+0x8]"); // Get the message code from the stack);
                fasm.AddLine("cmp eax, " + m_CustomMessageCode); // Check if the message code is our custom one
                fasm.AddLine("jne @call_original"); // Otherwise simply call the original WndProc
                fasm.AddLine("mov eax, [esp+0xC]"); // Function pointer
                fasm.AddLine("mov edx, [esp+0x10]"); // Result pointer
                fasm.AddLine("push edx"); // Save result pointer
                fasm.AddLine("call eax"); // Call the user function
                fasm.AddLine("pop edx"); // Restore the result pointer
                fasm.AddLine("mov [edx], eax"); // Save user function result
                fasm.AddLine("xor eax, eax"); // We handled the message
                fasm.AddLine("retn");

                fasm.AddLine("@call_original:");
                fasm.AddLine("mov ecx, [esp+0x4]"); // Hwnd
                fasm.AddLine("mov edx, [esp+0x8]"); // Msg
                fasm.AddLine("mov esi, [esp+0xC]"); // WParam
                fasm.AddLine("mov edi, [esp+0x10]"); // LParam


                fasm.AddLine("mov eax, [" + m_OriginalWndProc + "]");
                fasm.AddLine("push edi"); // LParam
                fasm.AddLine("push esi"); // WParam
                fasm.AddLine("push edx"); // Msg
                fasm.AddLine("push ecx"); // Hwnd
                fasm.AddLine("push eax"); // WndProc original

                //fasm.AddLine("call " + l_CallWindowProcW); // Call the original WndProc
                fasm.AddLine("retn 0x14");


                // Setup our WndProc callback
                //memory.WriteBytes(m_WndProcFunction, D3D.OriginalBytesDX);

                fasm.Inject(m_WndProcFunction);

                // Register our WndProc callback
                var fasm2 = new ManagedFasm(Memory.WowProcess.ProcessHandle);
                fasm2.SetMemorySize(0x500);
                fasm2.SetPassLimit(100);
                fasm2.AddLine("push " + m_WndProcFunction);
                fasm2.AddLine("push " + GWL_WNDPROC);
                fasm2.AddLine("push " + m_WindowHandle);
                fasm2.AddLine("call " + l_SetWindowLongW);
                fasm2.AddLine("mov [" + m_OriginalWndProc + "], eax");
                fasm2.AddLine("retn");

                var ptrInject = memory.AllocateMemory(0x500);
                fasm2.InjectAndExecute(ptrInject);

                /*var l_Process = System.Diagnostics.Process.GetProcessesByName("Wow").First();
                Console.WriteLine("Using process : " + l_Process.Id + ", window " + l_Process.MainWindowHandle + "");

                using (RemoteProcess l_RemoteProcess = new RemoteProcess((uint)l_Process.Id))
                using (var l_WndProcExecutor = new WndProcExecutor2(l_RemoteProcess, l_Process.MainWindowHandle))
                {

                    LuaTest(l_RemoteProcess, l_WndProcExecutor, "print(\"Hello world motha !\")");

                }*/
                /*var t = new MyMemory.RemoteProcess((uint)Memory.WowProcess.ProcessId);
                var remoteM = t.MemoryManager.AllocateMemory(0x1000);
                var ptrInject = (uint)remoteM.Pointer; //memory.AllocateMemory(0x500);

                fasm2.Inject(ptrInject);
                var prot = new RemoteMemoryProtection(t, remoteM.Pointer, remoteM.Size, Enumerations.MemoryProtectionFlags.Execute);
                
                var th = t.ThreadsManager.CreateRemoteThread((IntPtr) ptrInject, IntPtr.Zero, Enumerations.ThreadCreationFlags.Run);
                */
            }
            catch (Exception e)
            {
                Logging.WriteError(e.ToString());
            }
        }

        /// <summary>
        /// Example that call FrameScript__ExecuteBuffer
        /// </summary>
        /// <param name="p_Process">The remote process</param>
        /// <param name="p_Executor">The WndProc executor to use</param>
        /// <param name="p_Lua">The actual LUA code</param>
        private static void LuaTest(RemoteProcess p_Process, WndProcExecutor2 p_Executor, string p_Lua)
        {
            var l_FrameScript__ExecuteBuffer = p_Process.ModulesManager.MainModule.BaseAddress + 0x1A118F;
            var l_LuaBufferUTF8 = Encoding.UTF8.GetBytes(p_Lua);

            using (var l_RemoteBuffer = p_Process.MemoryManager.AllocateMemory((uint) l_LuaBufferUTF8.Length + 1))
            {
                l_RemoteBuffer.WriteBytes(l_LuaBufferUTF8);

                var l_Mnemonics = new string[]
                {
                    "push 0",
                    "push " + l_RemoteBuffer.Pointer,
                    "push " + l_RemoteBuffer.Pointer,
                    "call " + l_FrameScript__ExecuteBuffer,
                    "add esp, 0xC",
                    "retn"
                };

                p_Executor.Call(l_Mnemonics);
            }
        }

        public void Dispose()
        {
            IntPtr l_User32 = GetModuleHandle("user32.dll");
            IntPtr l_SetWindowLongW = GetProcAddress(l_User32, "SetWindowLongW");

            // Restore the original WndProc callback
            var fasm2 = new ManagedFasm(Memory.WowProcess.ProcessHandle);
            fasm2.SetMemorySize(0x500);
            fasm2.SetPassLimit(100);
            fasm2.AddLine("mov eax, [" + m_OriginalWndProc + "]");
            fasm2.AddLine("push eax");
            fasm2.AddLine("push " + GWL_WNDPROC + "");
            fasm2.AddLine("push " + m_WindowHandle + "");
            fasm2.AddLine("call " + l_SetWindowLongW + "");
            fasm2.AddLine("retn");
            var ptrInject = Memory.WowMemory.Memory.AllocateMemory(0x500);
            fasm2.InjectAndExecute(ptrInject);

            Memory.WowMemory.Memory.FreeMemory(m_WndProcFunction);
            Memory.WowMemory.Memory.FreeMemory(m_OriginalWndProc);
        }

        /// <summary>
        /// Call the specifics asm mnemonics in the message handler thread
        /// </summary>
        /// <param name="p_Mnemonics"></param>
        /// <param name="p_BufferSize"></param>
        /// <returns></returns>
        public uint Call(string[] p_Mnemonics, int p_BufferSize = 0x1000)
        {
            using (var fasm = new ManagedFasm(Memory.WowProcess.ProcessHandle))
            {
                fasm.SetMemorySize(0x500);
                fasm.SetPassLimit(100);
                foreach (var s in p_Mnemonics)
                {
                    fasm.AddLine(s);
                }
                uint ptrInject = Memory.WowMemory.Memory.AllocateMemory(p_BufferSize);
                fasm.Inject(ptrInject);
                return Call(ptrInject);
            }
        }

        /// <summary>
        /// Call the specific function in the message handler thread
        /// The function pointer is passed in WParam
        /// The result pointer is passed in LParam
        /// </summary>
        /// <param name="p_Function">The function pointer to call</param>
        /// <returns>The value of EAX after calling the function</returns>
        public uint Call(uint p_Function)
        {
            uint ptrResult = Memory.WowMemory.Memory.AllocateMemory(IntPtr.Size);
            SendMessage(m_WindowHandle, (uint) m_CustomMessageCode, (IntPtr) p_Function, (IntPtr) ptrResult);

            return Memory.WowMemory.Memory.ReadUInt(ptrResult);
        }
    }
}