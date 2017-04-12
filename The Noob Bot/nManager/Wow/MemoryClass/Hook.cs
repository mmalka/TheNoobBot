using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Fasm;
using nManager.Helpful;
using nManager.Wow.MemoryClass.Magic;
using nManager.Wow.Patchables;
using SlimDX.Direct3D11;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.MemoryClass
{
    /// <summary>
    ///     Hook endscene for injection
    /// </summary>
    public class Hook
    {
        /// <summary>
        ///     Locker Hook
        /// </summary>
        public static readonly object Locker = new object();

        private readonly BlackMagic _memory = new BlackMagic();
        public bool AllowReHook = false;
        internal uint JumpAddress;
        internal uint JumpAddressDX;

        /// <summary>
        ///     Thread Hooked
        /// </summary>
        public bool ThreadHooked;

        private uint _mExecuteRequested;
        private uint _mExecuteRequestedDX;
        private uint _mInjectionCode;
        private uint _mSavedAntiban;
        private uint _mLocked;
        private uint _mLockedDX;
        private uint _mResult;
        private uint _mTrampoline;
        private uint _mTrampolineDX;
        private byte[] _mZeroBytesInjectionCodes;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Hook" /> class.
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

        /// <value>
        ///     BlackMagic - Memory lib.
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

        private void CreateTrampolineDX()
        {
            _mTrampolineDX = Memory.AllocateMemory(0x1000);

            Console.WriteLine("m_trampoline : " + _mTrampolineDX.ToString("X"));

            var fasm = new ManagedFasm(Memory.ProcessHandle);

            fasm.SetMemorySize(0x1000);
            fasm.SetPassLimit(100);
            fasm.AddLine("pushad");
            fasm.AddLine("pushfd");
            fasm.AddLine("mov eax, [{0}]", _mLockedDX);
            fasm.AddLine("@execution:");

            fasm.AddLine("mov eax, [{0}]", _mExecuteRequestedDX);
            fasm.AddLine("test eax, eax");
            fasm.AddLine("je @lockcheck");
            fasm.AddLine("mov ebx, [{0}]", (Wow.Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.SpellChecker));
            fasm.AddLine("mov eax, [ebx+" + (uint) Addresses.FunctionWow.SpellCheckerOff1 + "]");
            fasm.AddLine("mov esi, [ebx+" + (uint) Addresses.FunctionWow.SpellCheckerOff2 + "]");
            fasm.AddLine("mov [" + _mSavedAntiban + "], esi");
            fasm.AddLine("mov [ebx+" + (uint) Addresses.FunctionWow.SpellCheckerOff2 + "], eax");
            fasm.AddLine("call {0}", _mInjectionCode);
            fasm.AddLine("mov [" + _mResult + "], eax");
            fasm.AddLine("mov edx, {0}", (uint) (Wow.Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.CTMChecker));
            fasm.AddLine("call " + (uint) (Wow.Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.WoWTextCaller));
            fasm.AddLine("push happilyeverafter");
            fasm.AddLine("push " + (uint) (Wow.Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.RetFromFunctionBelow));
            fasm.AddLine("jmp " + (uint) (Wow.Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.CTMChecker2));
            fasm.AddLine("happilyeverafter:");
            fasm.AddLine("mov ebx, [{0}]", (Wow.Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.SpellChecker));
            fasm.AddLine("mov esi, [" + _mSavedAntiban + "]");
            fasm.AddLine("mov [ebx+" + (uint) Addresses.FunctionWow.SpellCheckerOff2 + "], esi");
            fasm.AddLine("xor eax, eax");
            fasm.AddLine("mov [" + _mExecuteRequestedDX + "], eax");

            fasm.AddLine("@lockcheck:");

            fasm.AddLine("mov eax, [{0}]", _mLockedDX);
            fasm.AddLine("test eax, eax");
            fasm.AddLine("jne @execution");
            fasm.AddLine("push 0");
            fasm.AddLine("add esp, 4");
            fasm.AddLine("popfd");
            fasm.AddLine("popad");

            Memory.WriteBytes(_mTrampolineDX, D3D.OriginalBytesDX);
            fasm.AddLine("jmp " + (JumpAddressDX + D3D.OriginalBytesDX.Length));
            fasm.Inject((uint) (_mTrampolineDX + D3D.OriginalBytesDX.Length));
        }

        private void CreateTrampoline()
        {
            _mTrampoline = Memory.AllocateMemory(0x1000);

            Console.WriteLine("m_trampoline : " + _mTrampoline.ToString("X"));

            var fasm = new ManagedFasm(Memory.ProcessHandle);

            fasm.SetMemorySize(0x1000);
            fasm.SetPassLimit(100);
            fasm.AddLine("call {0}", Wow.Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.ReturnFunc);
            fasm.AddLine("pushad");
            fasm.AddLine("pushfd");

            fasm.AddLine("mov eax, [{0}]", _mLocked);
            fasm.AddLine("@execution:");

            fasm.AddLine("mov eax, [{0}]", _mExecuteRequested);
            fasm.AddLine("test eax, eax");
            fasm.AddLine("je @lockcheck");

            fasm.AddLine("call {0}", _mInjectionCode);
            fasm.AddLine("mov [" + _mResult + "], eax");
            fasm.AddLine("xor eax, eax");
            fasm.AddLine("mov [" + _mExecuteRequested + "], eax");

            fasm.AddLine("@lockcheck:");

            fasm.AddLine("mov eax, [{0}]", _mLocked);
            fasm.AddLine("test eax, eax");
            fasm.AddLine("jne @execution");

            fasm.AddLine("popfd");
            fasm.AddLine("popad");

            fasm.AddLine("jmp " + (JumpAddress + D3D.OriginalBytes.Length));
            fasm.Inject(_mTrampoline);
        }

        public uint InjectAndExecute(string[] asm)
        {
            lock (Locker)
            {
                if (!ThreadHooked)
                    return 0;
                var fasm = new ManagedFasm(Memory.ProcessHandle);

                fasm.SetMemorySize(0x1000);
                fasm.SetPassLimit(100);

                foreach (string s in asm)
                {
                    fasm.AddLine(s);
                }

                fasm.Inject(_mInjectionCode);

                Memory.WriteByte(_mExecuteRequested, 1);
                Timer injectTimer = new Timer(2000);
                injectTimer.Reset();
                while (Memory.ReadByte(_mExecuteRequested) == 1 && !injectTimer.IsReady)
                {
                    Thread.Sleep(1);
                }

                if (injectTimer.IsReady)
                {
                    Logging.WriteError("Injection have been aborted, execution too long from " + CurrentCallStack);
                    return 0;
                }
                Memory.WriteBytes(_mInjectionCode, _mZeroBytesInjectionCodes);

                uint returnValue = Memory.ReadUInt(_mResult);
                return returnValue;
            }
        }

        public static string CurrentCallStack
        {
            get
            {
                string myStack = "";
                for (int i = 10; i >= 1; i--)
                {
                    var stackFrame = new StackFrame(i);
                    if (stackFrame.GetMethod() != null)
                        myStack = myStack + stackFrame.GetMethod().Name + " => ";
                }
                myStack = myStack.Substring(0, myStack.Length - 4);
                return myStack;
            }
        }

        public void ApplyDX()
        {
            var fasm = new ManagedFasm(Memory.ProcessHandle);

            fasm.SetMemorySize(0x1000);
            fasm.SetPassLimit(100);

            fasm.AddLine("jmp " + _mTrampolineDX);
            if (D3D.OriginalBytesDX.Length > 5)
                fasm.AddLine("nop");
            if (D3D.OriginalBytesDX.Length > 6)
                fasm.AddLine("nop");

            fasm.Inject(JumpAddressDX);
        }

        public void Apply()
        {
            var fasm = new ManagedFasm(Memory.ProcessHandle);

            fasm.SetMemorySize(0x1000);
            fasm.SetPassLimit(100);

            fasm.AddLine("jmp " + _mTrampoline);

            fasm.Inject(JumpAddress);
        }

        public void Remove(uint address, byte[] originalBytes)
        {
            Memory.WriteBytes(address, originalBytes);
        }

        public bool IsGameFrameLocked
        {
            get { return Memory.ReadByte(_mLocked) == 1; }
        }

        private int _lockRequests;

        public void GameFrameUnLock()
        {
            lock (Locker)
            {
                if (_lockRequests > 0)
                {
                    Memory.WriteByte(_mLocked, 0);
                    _lockRequests = 0;
                }
                _lockRequests--;
                if (_lockRequests < 0)
                    _lockRequests = 0;
            }
        }

        public void GameFrameLock()
        {
            lock (Locker)
            {
                if (!nManagerSetting.CurrentSetting.UseFrameLock)
                    return;
                _lockRequests++;
                Memory.WriteByte(_mLocked, 1);
                //Memory.WriteByte(_mLockRequested, 1);
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
                        string textBuild = Memory.ReadUTF8String(Wow.Memory.WowProcess.WowModule + (uint) Addresses.GameInfo.buildWoWVersionString);
                        uint wowBuildVersion = Helpers.Usefuls.WowVersion(textBuild);
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
                        JumpAddressDX = GetJumpAdresseDX();

                        if (Memory.ReadByte(JumpAddress) == 0xE9 || Memory.ReadByte(JumpAddressDX) == 0xE9)
                        {
                            DisposeHooking();
                        }
                        try
                        {
                            if (D3D.OriginalBytes == null)
                            {
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

                                D3D.OriginalBytes = Memory.ReadBytes(JumpAddress, 5); // WinXP - Win7, with standards graphic drivers.
                                D3D.OriginalBytesDX = Memory.ReadBytes(JumpAddressDX, 5); // WinXP - Win7, with standards graphic drivers.
                                if (D3D.OriginalBytesDX[0] == 0x55)
                                    D3D.OriginalBytesDX = Memory.ReadBytes(JumpAddressDX, 6); // WinXP - Win7, with specific graphic drivers.
                                else if (D3D.OriginalBytesDX[0] == 0x6A)
                                    D3D.OriginalBytesDX = Memory.ReadBytes(JumpAddressDX, 7); // Win8, add 2 nop to fit 5 bytes for UnHook.
                            }
                            //_mLockRequested = Memory.AllocateMemory(0x4);
                            _mLocked = Memory.AllocateMemory(0x4);
                            _mLockedDX = Memory.AllocateMemory(0x4);
                            _mResult = Memory.AllocateMemory(0x4);
                            _mSavedAntiban = Memory.AllocateMemory(0x4);
                            _mExecuteRequested = Memory.AllocateMemory(0x4);
                            _mExecuteRequestedDX = Memory.AllocateMemory(0x4);

                            _mZeroBytesInjectionCodes = new byte[0x1000];
                            for (int i = 0; i < 0x1000; i++)
                            {
                                _mZeroBytesInjectionCodes[i] = 0;
                            }

                            _mInjectionCode = Memory.AllocateMemory(_mZeroBytesInjectionCodes.Length);
                            CreateTrampoline();
                            CreateTrampolineDX();
                            Apply();
                            ApplyDX();
                        }
                        catch (Exception e)
                        {
                            Logging.WriteError("Hooking()#1: " + e);
                            ThreadHooked = false;
                            return;
                        }
                    }
                    ThreadHooked = true;
                    AllowReHook = true;
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
            if (jmp == 0xE9)
                return;
            if (!AllowReHook)
                return;
            ThreadHooked = false;
            AllowReHook = false;
            Logging.WriteError("ThreadHooked: UnHooked; JmpAddress: " + jmp.ToString("X") + ", trying to reHook.");
            Hooking();
        }

        private uint GetJumpAdresse()
        {
            try
            {
                return Wow.Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.GetTargetInfo;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetJumpAdresse(): " + e);
            }
            return 0;
        }

        private uint GetJumpAdresseDX()
        {
            try
            {
                if (D3D.IsD3D11(Memory.ProcessId))
                    return D3D.D3D11Adresse();
                return D3D.D3D9Adresse(Memory.ProcessId);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetJumpAdresse(): " + e);
            }
            return 0;
        }

        /// <summary>
        ///     Disposes the hook.
        /// </summary>
        internal void DisposeHooking()
        {
            try
            {
                if (!Memory.IsProcessOpen)
                    return;
                // Get address of EndScene:
                JumpAddress = GetJumpAdresse();
                JumpAddressDX = GetJumpAdresseDX();

                if (Memory.ReadByte(JumpAddress) == 0xE9)
                {
                    lock (Locker)
                    {
                        // Restore origine endscene:
                        if (D3D.OriginalBytesDX == null)
                        {
                            D3D.OriginalBytesDX = Memory.ReadBytes(JumpAddress, 5);
                            byte[] wrongdata = {0, 0, 0, 0, 0};
                            if (D3D.OriginalBytesDX == wrongdata)
                            {
                                Others.OpenWebBrowserOrApplication("http://thenoobbot.com/community/viewtopic.php?f=43&t=464");
                                Logging.Write("An error is detected, you must switch the DirectX version used by your WoW client !");
                                MessageBox.Show("An error is detected, you must switch the DirectX version used by your WoW client !");
                                Pulsator.Dispose(true);
                                return;
                            }
                            // on the first hooking, we add 0 nop if 5bytes reading, 1nop if 6 bytes, 2nop if 7 bytes,
                            // that's why we need to read 9 here to be able to detect the 7bytes hooking.
                            // nop = 144, if there is no nop, that mean we are in a normal 5bytes mode.
                            byte[] getBytes = Memory.ReadBytes(JumpAddressDX, 9);
                            if (getBytes[5] != 144 && getBytes[6] != 144)
                                D3D.OriginalBytesDX = new byte[] {139, 255, 85, 139, 236}; // WinXP/WinVista/Win7
                            else if (getBytes[5] == 144 && getBytes[6] != 144)
                                D3D.OriginalBytesDX = new byte[] {85, 139, 236, 139, 69, 8}; // Some graphic drivers
                            else if (getBytes[5] == 144 && getBytes[6] == 144)
                                D3D.OriginalBytesDX = new byte[] {106, 20, 184, 12, 154, 68, 115}; // Win8
                                // the 2 lasts bytes of the Win8 way seems to be differents on differents computers.
                            else
                            {
                                string bytes = "";
                                foreach (uint bit in getBytes)
                                {
                                    if (bytes == "")
                                        bytes = bit.ToString();
                                    else
                                        bytes = bytes + ", " + bit;
                                }
                                Logging.WriteError("Error Hook_01 : Couldn't dispose previous Hooking correctly, please open a bug report thread on the forum with this log file.");
                                Logging.WriteError("Error Hook_02 : " + bytes);
                                Others.OpenWebBrowserOrApplication("http://thenoobbot.com/community/viewtopic.php?f=43&t=464");
                                MessageBox.Show(
                                    "World of Warcraft is currently in use by another Application than TheNoobBot and we could not automaticallt unhook it, try restarting the WoW Client, if this issue persist, open a bug report with this log file.");
                                Pulsator.Dispose(true);
                            }
                        }
                        //D3D.OriginalBytes = new byte[] {0xE8, 0x71, 0xCF, 0x0D, 0x00};
                        //Remove(JumpAddress, D3D.OriginalBytes);
                        var fasm = new ManagedFasm(Memory.ProcessHandle);
                        fasm.SetMemorySize(0x1000);
                        fasm.SetPassLimit(100);
                        fasm.AddLine("call {0}", Wow.Memory.WowProcess.WowModule + (uint)Addresses.FunctionWow.ReturnFunc);
                        fasm.Inject(JumpAddress);
                        Remove(JumpAddressDX, D3D.OriginalBytesDX);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("DisposeHooking(): " + e);
            }
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
                var memory = new BlackMagic(processId);
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
                var memory = new BlackMagic(processId);
                // 
                System.Diagnostics.Process processById = System.Diagnostics.Process.GetProcessById(processId);
                uint baseModule = 0;
                foreach (ProcessModule v in from ProcessModule v in memory.Modules where String.Equals(v.ModuleName, (processById.ProcessName + ".exe"), StringComparison.CurrentCultureIgnoreCase) select v)
                {
                    baseModule = (uint) v.BaseAddress;
                }

                string pName = memory.ReadUTF8String(baseModule + (uint) Addresses.Player.playerName);
                if (!String.IsNullOrEmpty(pName))
                    return pName;
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
                var memory = new BlackMagic(processId);
                // 
                System.Diagnostics.Process processById = System.Diagnostics.Process.GetProcessById(processId);
                uint baseModule = 0;
                foreach (ProcessModule v in from ProcessModule v in memory.Modules where String.Equals(v.ModuleName, (processById.ProcessName + ".exe"), StringComparison.CurrentCultureIgnoreCase) select v)
                {
                    baseModule = (uint) v.BaseAddress;
                }
                return (memory.ReadInt(baseModule + (uint) Addresses.GameInfo.isLoading) == 0) &&
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