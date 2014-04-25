using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Fasm;
using nManager.Helpful;
using nManager.Wow.MemoryClass.Magic;
using nManager.Wow.Patchables;

namespace nManager.Wow.MemoryClass
{
    /// <summary>
    /// Hook endscene for injection
    /// </summary>
    public class Hook
    {
        /// <summary>
        /// Locker Hook
        /// </summary>
        public static readonly object Locker = new object();

        private uint _addresseInjection;

        private readonly BlackMagic _memory = new BlackMagic();
        internal uint InjectedCodeDetour;
        internal uint JumpAddress;
        private uint _retnInjectionAsm;
        private uint _startInject;

        /// <summary>
        /// Thread Hoocked
        /// </summary>
        public bool ThreadHooked;

        public int OffsetHookMemoryAccess = 0xB5;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hook"/> class.
        /// </summary>
        public Hook()
        {
            try
            {
                Hooking();
            }
            catch (Exception e)
            {
                Logging.WriteError("Hook(): " + e);
            }
        }

        /// <summary>
        /// BlackMagic - Memory lib.
        /// </summary>
        /// <value>
        /// BlackMagic - Memory lib.
        /// </value>
        public BlackMagic Memory
        {
            get
            {
                try
                {
                    return _memory;
                }
                catch (Exception e)
                {
                    Logging.WriteError("BlackMagic Memory: " + e);
                    return new BlackMagic();
                }
            }
        }

        private void Hooking()
        {
            try
            {
                lock (Locker)
                {
                    if (Wow.Memory.WowProcess.ProcessId <= 0)
                        return;

                    // Process Connect:
                    if (!Memory.IsProcessOpen || Memory.ProcessId != Wow.Memory.WowProcess.ProcessId)
                    {
                        Memory.OpenProcessAndThread(Wow.Memory.WowProcess.ProcessId);
                    }

                    if (Memory.IsProcessOpen)
                    {
                        uint wowBuildVersion = Wow.Helpers.Usefuls.WowVersion;

                        if (wowBuildVersion != Information.TargetWowBuild)
                        {
                            if (System.Diagnostics.Process.GetProcessById(Memory.ProcessId).HasExited)
                                return;
                            if (wowBuildVersion == 0 || wowBuildVersion < Information.MinWowBuild || wowBuildVersion > Information.MaxWowBuild)
                            {
                                // Offsets seems to have changed.
                                MessageBox.Show(
                                    Translate.Get(Translate.Id.UpdateRequiredCases) + Environment.NewLine + Environment.NewLine + Translate.Get(Translate.Id.UpdateRequiredCase1) +
                                    Environment.NewLine + Translate.Get(Translate.Id.UpdateRequiredCase2),
                                    Translate.Get(Translate.Id.UpdateRequiredCasesTitle));
                                return;
                            }
                            // Offsets has not changed, but may have a function offsets changes. We may need to create a integrated function offsets pattern finder in that case.
                            if (Information.TargetWowBuild > wowBuildVersion)
                            {
                                MessageBox.Show(
                                    Translate.Get(Translate.Id.UpdateRequireOlderTheNoobBot) + Information.TargetWowVersion + Translate.Get(Translate.Id.RunningWoWBuildDot) +
                                    Information.TargetWowBuild +
                                    Translate.Get(Translate.Id.RunningWoWBuild) + wowBuildVersion + Translate.Get(Translate.Id.RunningWoWBuildDot) + Environment.NewLine +
                                    Environment.NewLine + Translate.Get(Translate.Id.PleaseDownloadOlder), Translate.Get(Translate.Id.UpdateRequireOlderTheNoobBotTitle));
                            }
                            if (Information.TargetWowBuild < wowBuildVersion)
                            {
                                MessageBox.Show(
                                    Translate.Get(Translate.Id.UpdateRequireNewerTheNoobBot) + Information.TargetWowVersion + Translate.Get(Translate.Id.RunningWoWBuildDot) +
                                    Information.TargetWowBuild +
                                    Translate.Get(Translate.Id.RunningWoWBuild) + wowBuildVersion + Translate.Get(Translate.Id.RunningWoWBuildDot) + Environment.NewLine +
                                    Environment.NewLine + Translate.Get(Translate.Id.PleaseDownloadNewer), Translate.Get(Translate.Id.UpdateRequireNewerTheNoobBotTitle));
                            }
                            return;
                        }

                        // Get address of EndScene
                        JumpAddress = GetJumpAdresse();

                        if (Memory.ReadByte(JumpAddress) == 0xE9 && (InjectedCodeDetour == 0 || _addresseInjection == 0))
                            // check if wow is already hooked and dispose Hook
                        {
                            DisposeHooking();
                        }

                        if (Memory.ReadByte(JumpAddress) != 0xE9 || true)
                            // check if wow is already hooked // TODO try fix error rehook
                        {
                            try
                            {
                                _startInject = (uint) Others.Random(0, 60);


                                ThreadHooked = false;
                                // allocate memory to store injected code:
                                InjectedCodeDetour = Memory.AllocateMemory(4000 + Others.Random(1, 2000)) +
                                                     _startInject;
                                // allocate memory the new injection code pointer:
                                _addresseInjection = Memory.AllocateMemory(0x4);
                                Memory.AllocateMemory(0x4);
                                Memory.WriteInt(_addresseInjection, 0);
                                // allocate memory the pointer return value:
                                _retnInjectionAsm = Memory.AllocateMemory(0x4);
                                Memory.WriteInt(_retnInjectionAsm, 0);

                                // Generate the STUB to be injected
                                Memory.Asm = new ManagedFasm(Memory.ProcessHandle);
                                Memory.Asm.Clear(); // $Asm

                                // save regs
                                int nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("pushfd");
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("pushad");

                                // Test if you need launch injected code:
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("mov eax, [" + _addresseInjection + "]");
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("test eax, eax");
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("je @out");

                                // Launch Fonction:
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("call eax");

                                // Copie pointer return value:
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("mov [" + _retnInjectionAsm + "], eax");

                                // Enter value 0 of addresse func inject
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("mov eax, " + _addresseInjection);
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("mov edx, 0");
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("mov [eax], edx");
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("mov eax, " + _addresseInjection);
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("mov ebx, 0");
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("mov [eax], ebx");

                                // Close func
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("@out:");

                                // load reg
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("popad");
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine("popfd");
                                nR = Others.Random(1, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }

                                // injected code

                                Memory.Asm.Inject(InjectedCodeDetour);
                                uint sizeAsm = (uint) (Memory.Asm.Assemble().Length);

                                // copy and save original instructions
                                Memory.Asm.Clear();


                                /*foreach (var opcode in D3D.OriginalOpcode)
                                {
                                    Memory.Asm.AddLine(opcode);
                                    nR = Others.Random(0, 5);
                                    for (int i = nR; i >= 1; i--)
                                    {
                                        Memory.Asm.AddLine(ProtectHook());
                                    }
                                }*/
                                if (D3D.OriginalBytes == null)
                                {
                                    D3D.OriginalBytes = Memory.ReadBytes(JumpAddress, 5);
                                    byte[] wrongdata = new byte[] {0, 0, 0, 0, 0};
                                    if (D3D.OriginalBytes == wrongdata)
                                    {
                                        Others.OpenWebBrowserOrApplication("http://theprivatebot.com/community/viewtopic.php?f=43&t=464");
                                        Logging.Write("An error is detected, you must switch the DirectX version used by your WoW client !");
                                        MessageBox.Show("An error is detected, you must switch the DirectX version used by your WoW client !");
                                        Pulsator.Dispose(true);
                                    }
                                    byte[] extractAllBytes = Memory.ReadBytes(JumpAddress, 10);
                                    // Gather as much data as possible if there is others graphic cards system.
                                    string bytes = "";
                                    foreach (uint bit in extractAllBytes)
                                    {
                                        if (bytes == "")
                                            bytes = bit.ToString();
                                        else
                                            bytes = bytes + ", " + bit;
                                    }
                                    Logging.WriteFileOnly("Hooking Informations: " + bytes);
                                    if (D3D.OriginalBytes[0] != 0xE9)
                                    {
                                        if (D3D.OriginalBytes[0] == 85)
                                            D3D.OriginalBytes = Memory.ReadBytes(JumpAddress, 6);
                                        else if (D3D.OriginalBytes[0] == 106)
                                            D3D.OriginalBytes = Memory.ReadBytes(JumpAddress, 7);
                                    }
                                    else
                                    {
                                        // on the first hooking, we add 0 nop if 5bytes reading, 1nop if 6 bytes, 2nop if 7 bytes,
                                        // that's why we need to read 9 here to be able to detect the 7bytes hooking.
                                        // nop = 144, if there is no nop, that mean we are in a normal 5bytes mode.
                                        byte[] getBytes = Memory.ReadBytes(JumpAddress, 9);
                                        if (getBytes[5] != 144 && getBytes[6] != 144)
                                            D3D.OriginalBytes = new byte[] {139, 255, 85, 139, 236}; // WinXP/WinVista/Win7
                                        else if (getBytes[5] == 144 && getBytes[6] != 144)
                                            D3D.OriginalBytes = new byte[] {85, 139, 236, 139, 69, 8}; // Some graphic drivers
                                        else if (getBytes[5] == 144 && getBytes[6] == 144)
                                            D3D.OriginalBytes = new byte[] {106, 20, 184, 12, 154, 68, 115}; // Win8
                                            // the 2 lasts bytes of the Win8 way seems to be differents on differents computers.
                                        else
                                        {
                                            Others.OpenWebBrowserOrApplication("http://theprivatebot.com/community/viewtopic.php?f=43&t=464");
                                            Logging.Write("An error is detected, please restart your WoW Client before running the bot again !");
                                            MessageBox.Show("An error is detected, please restart your WoW Client before running the bot again !");
                                            Pulsator.Dispose(true);
                                        }
                                    }
                                }
                                int sizeJumpBack = D3D.OriginalBytes.Length;
                                Memory.WriteBytes(InjectedCodeDetour + sizeAsm, D3D.OriginalBytes);

                                //int sizeJumpBack = Memory.Asm.Assemble().Length;
                                //Memory.Asm.Inject(InjectedCodeDetour + sizeAsm);

                                // create jump back stub
                                Memory.Asm.Clear();
                                Memory.Asm.AddLine("jmp " + (JumpAddress + sizeJumpBack));
                                nR = Others.Random(0, 10);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }

                                Memory.Asm.Inject((InjectedCodeDetour + sizeAsm + (uint) sizeJumpBack));

                                // create hook jump
                                Memory.Asm.Clear(); // $jmpto
                                Memory.Asm.AddLine("jmp " + (InjectedCodeDetour));
                                if (sizeJumpBack >= 6)
                                    Memory.Asm.AddLine("nop");
                                if (sizeJumpBack == 7)
                                    Memory.Asm.AddLine("nop");
                                Memory.Asm.Inject(JumpAddress);

                                // add nop if needed
                                Memory.Asm.Clear();
                            }
                            catch (Exception e)
                            {
                                Logging.WriteError("Hooking()#1: " + e);
                                ThreadHooked = false;
                                return;
                            }
                        }
                        ThreadHooked = true;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Hooking()#2: " + e);
            }
        }

        private void CheckEndsceneHook()
        {
            byte jmp = Memory.ReadByte(JumpAddress);
            if (jmp == 0xE9) return;
            ThreadHooked = false;
            Logging.WriteError("ThreadHooked: False; JmpAddress: " + jmp);
        }

        private uint GetJumpAdresse()
        {
            try
            {
                if (D3D.IsD3D11(Memory.ProcessId))
                    return D3D.D3D11Adresse();
                else
                    return D3D.D3D9Adresse(Memory.ProcessId);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetJumpAdresse(): " + e);
            }
            return 0;
        }

        /// <summary>
        /// Get Random ASM line.
        /// </summary>
        /// <returns></returns>
        internal static string ProtectHook()
        {
            List<string> asm = new List<string>
            {
                "mov edx, edx",
                "mov edi, edi",
                "xchg ebp, ebp",
                "mov esp, esp",
                "xchg esp, esp",
                "xchg edx, edx",
                "mov edi, edi"
            };


            // asm.Add("nop");

            // asm.Add( "mov eax, eax");

            //   asm.Add("xchg eax, eax");

            try
            {
                int n = Others.Random(0, asm.Count - 1);
                return asm[n];
            }
            catch (Exception e)
            {
                Logging.WriteError("ProtectHook(): " + e);

                return "mov eax, eax";
            }
        }

        /// <summary>
        /// Disposes the hook.
        /// </summary>
        internal void DisposeHooking()
        {
            try
            {
                if (!Memory.IsProcessOpen)
                    return;
                // Get address of EndScene:
                JumpAddress = GetJumpAdresse();

                if (Memory.ReadByte(JumpAddress) == 0xE9) // check if wow is already hooked and dispose Hook
                {
                    lock (Locker)
                    {
                        // Restore origine endscene:
                        if (D3D.OriginalBytes != null)
                            Memory.WriteBytes(JumpAddress, D3D.OriginalBytes);
                    }
                }

                // free memory:
                Memory.FreeMemory(InjectedCodeDetour - _startInject);
                Memory.FreeMemory(_addresseInjection);
                Memory.FreeMemory(_retnInjectionAsm);
            }
            catch (Exception e)
            {
                Logging.WriteError("DisposeHooking(): " + e);
            }
        }

        /// <summary>
        /// Injects the and execute Asm lines.
        /// </summary>
        /// <param name="asm">The asm code.</param>
        /// <param name="returnValue">if set to <c>true</c> [return value].</param>
        /// <param name="returnLength">Length of the return.</param>
        /// <returns></returns>
        public byte[] InjectAndExecute(IEnumerable<string> asm, bool returnValue = false, int returnLength = 0)
        {
            try
            {
                lock (Locker)
                {
                    byte[] tempsByte = new byte[0];
                    try
                    {
                        // Hook Wow:
                        CheckEndsceneHook();
                        if (!Memory.IsProcessOpen || !ThreadHooked)
                            Hooking();

                        if (Memory.IsProcessOpen && ThreadHooked)
                        {
                            // Write the asm stuff
                            Memory.Asm.Clear();
                            foreach (string tempLineAsm in asm)
                            {
                                int nR = Others.Random(0, 3);
                                for (int i = nR; i >= 1; i--)
                                {
                                    Memory.Asm.AddLine(ProtectHook());
                                }
                                Memory.Asm.AddLine(tempLineAsm);
                            }


                            // Allocation Memory
                            uint startBaseInject = (uint) Others.Random(0, 60);
                            uint injectionAsmCodecave =
                                Memory.AllocateMemory(Memory.Asm.Assemble().Length + Others.Random(60, 80)) +
                                startBaseInject;
                            if (injectionAsmCodecave <= startBaseInject)
                            {
                                return tempsByte;
                            }

                            // Inject
                            Memory.Asm.Inject(injectionAsmCodecave);

                            if (true) // !Helpers.Usefuls.IsLoadingOrConnecting || 
                            {
                                Memory.WriteUInt(_addresseInjection, injectionAsmCodecave);
                                //Process.InjectionCount.Add(Others.Times);
                                while (Memory.ReadInt(_addresseInjection) > 0)
                                {
                                    Thread.Sleep(1);
                                } // Wait to launch code

                                if (!returnValue)
                                {
                                    tempsByte = new byte[0];
                                }
                                else if (returnLength > 0)
                                {
                                    tempsByte = Memory.ReadBytes(Memory.ReadUInt(_retnInjectionAsm), returnLength);
                                }
                                else
                                {
                                    List<byte> retnByte = new List<byte>();
                                    uint dwAddress = Memory.ReadUInt(_retnInjectionAsm);
                                    byte buf = Memory.ReadByte(dwAddress);
                                    while (buf != 0)
                                    {
                                        retnByte.Add(buf);
                                        dwAddress = dwAddress + 1;
                                        buf = Memory.ReadByte(dwAddress);
                                    }
                                    tempsByte = retnByte.ToArray();
                                }
                            }
                            Memory.WriteInt(_retnInjectionAsm, 0);


                            // Free memory allocated 
                            Memory.FreeMemory(injectionAsmCodecave - startBaseInject);
                        }
                        // return
                        return tempsByte;
                    }
                    catch
                    {
                        Logging.WriteError("Error injection");
                        Memory.WriteInt(_retnInjectionAsm, 0);
                        return new byte[0];
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "InjectAndExecute(IEnumerable<string> asm, bool returnValue = false, int returnLength = 0): " + e);
            }
            return new byte[0];
        }

        public static bool WowIsUsed(int processId)
        {
            try
            {
                uint pJump = 0;
                if (D3D.IsD3D11(processId))
                    pJump = D3D.D3D11Adresse();
                else
                    pJump = D3D.D3D9Adresse(processId);
                BlackMagic memory = new BlackMagic(processId);
                return memory.ReadByte(pJump) == 0xE9;
            }
            catch (Exception e)
            {
                Logging.WriteError("WowIsUsed(int processId): " + e);
            }
            return false;
        }

        public static string PlayerName(int processId)
        {
            try
            {
                if (!IsInGame(processId))
                    return Translate.Get(Translate.Id.Please_connect_to_the_game);

                // init memory
                BlackMagic memory = new BlackMagic(processId);
                // 
                uint baseModule = 0;
                foreach (ProcessModule v in from ProcessModule v in memory.Modules where v.ModuleName.ToLower() == "wow.exe" || v.ModuleName.ToLower() == "pandawow.exe" select v)
                {
                    baseModule = (uint) v.BaseAddress;
                }
                return memory.ReadUTF8String(baseModule + (uint) Addresses.Player.playerName);
            }
            catch (Exception e)
            {
                Logging.WriteError("PlayerName(int processId): " + e);
            }
            return "No Name";
        }

        public static bool IsInGame(int processId)
        {
            try
            {
                // init memory
                BlackMagic memory = new BlackMagic(processId);
                // 
                uint baseModule = 0;
                foreach (ProcessModule v in from ProcessModule v in memory.Modules where v.ModuleName.ToLower() == "wow.exe" || v.ModuleName.ToLower() == "pandawow.exe" select v)
                {
                    baseModule = (uint) v.BaseAddress;
                }
                return (memory.ReadInt(baseModule + (uint) Addresses.GameInfo.isLoadingOrConnecting) == 0) &&
                       (memory.ReadByte(baseModule + (uint) Addresses.GameInfo.gameState) > 0);
            }
            catch (Exception e)
            {
                Logging.WriteError("IsInGame(int processId): " + e);
            }
            return false;
        }
    }
}