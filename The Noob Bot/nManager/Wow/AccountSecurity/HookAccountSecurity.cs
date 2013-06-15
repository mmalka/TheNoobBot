using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.MemoryClass;
using nManager.Wow.Patchables;
using Timer = nManager.Helpful.Timer;
using Usefuls = nManager.Wow.Helpers.Usefuls;

namespace nManager.Wow.AccountSecurity
{
    internal class HookAccountSecurity
    {
        #region address

        private static uint _codeCaveDetourPtr;
        private static uint _codeCaveScanDump;
        internal static uint ScanFunction;
        internal static uint CurrentAddressReadDump;
        internal static uint StartDetourPtr;

        #endregion

        [StructLayout(LayoutKind.Explicit, Size = 0x8)]
        public struct DumpScan
        {
            [FieldOffset(0x0)] public uint Address;
            [FieldOffset(0x4)] public int Length;
        }

        private static bool _accountSecurityThreadIsAlive;

        internal static void Pulse()
        {
            try
            {
                lock (typeof (HookAccountSecurity))
                {
                    if (!_accountSecurityThreadIsAlive)
                    {
                        _accountSecurityThreadIsAlive = true;
                        var checkUpdateThreadLaunch = new Thread(LoopAccountSecurityThread)
                            {
                                Name =
                                    "loopAccountSecurity"
                            };
                        checkUpdateThreadLaunch.Start();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("HookAccountSecurity > Pulse(): " + exception);
            }
        }

        private static void LoopAccountSecurityThread()
        {
            try
            {
                while (true)
                {
                    if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting && Memory.WowMemory.ThreadHooked)
                    {
                        if (Hook())
                        {
                            if (Memory.WowMemory.Memory.ReadUInt(_codeCaveScanDump) > CurrentAddressReadDump)
                            {
                                var dumpScanTemps =
                                    (DumpScan)
                                    Memory.WowMemory.Memory.ReadObject(CurrentAddressReadDump, typeof (DumpScan));
                                if (dumpScanTemps.Length > 0)
                                {
                                    var specialAddress = GetSpecialAddressScan(dumpScanTemps);
                                    if (specialAddress != "")
                                    {
                                        Memory.WowProcess.KillWowProcess();
                                        Others.GetRequest("http://tech.thenoobbot.com/newScanAccountSecurity.php",
                                                          "msg=" + specialAddress);
                                        Logging.Write("Suspect activity at this adresse: " + specialAddress,
                                                      Logging.LogType.S, Color.Red);
                                        AccountSecurity.DialogNewScan();
                                    }
                                }

                                dumpScanTemps.Length = 0;
                                dumpScanTemps.Address = 0;
                                Memory.WowMemory.Memory.WriteObject(CurrentAddressReadDump, dumpScanTemps);

                                CurrentAddressReadDump = CurrentAddressReadDump +
                                                         (uint) Marshal.SizeOf(typeof (DumpScan));
                            }
                            else
                            {
                                if (Memory.WowMemory.Memory.ReadUInt(_codeCaveScanDump) >
                                    _codeCaveScanDump + 0x4 + (0x7*100000))
                                {
                                    Memory.WowMemory.Memory.WriteUInt(_codeCaveScanDump, _codeCaveScanDump + 0x4);
                                    Thread.Sleep(10);
                                    if (Memory.WowMemory.Memory.ReadUInt(_codeCaveScanDump) < _codeCaveScanDump)
                                    {
                                        CurrentAddressReadDump = _codeCaveScanDump + 0x4;
                                    }
                                }
                            }
                        }
                    }
                    Thread.Sleep(350); //150
                }
            }
            catch (Exception exception)
            {
                _accountSecurityThreadIsAlive = false;
                Logging.WriteError("loopAccountSecurityThread(): " + exception);
            }
        }

        private static bool Hook()
        {
            try
            {
                if (!Memory.WowMemory.ThreadHooked)
                    return false;

                if (AccountSecurityScanFonctionAddress() <= 0)
                    return false;

                lock (MemoryClass.Hook.Locker)
                {
                    if (Memory.WowMemory.Memory.ReadByte(ScanFunction) == 0x8B ||
                        (Memory.WowMemory.Memory.ReadByte(ScanFunction) == 0xE9 && _codeCaveScanDump <= 0 &&
                         _codeCaveDetourPtr <= 0))
                    {
                        /* ORIGINAL FUNCTION SCANAccountSecurity
        56                         - push esi
        57                         - push edi
        FC                         - cld 
        8B 54 24 14                - mov edx,[esp+14]
        8B 74 24 10                - mov esi,[esp+10]
        8B 44 24 0C                - mov eax,[esp+0C]
            8B CA                      - mov ecx,edx // Hook
            8B F8                      - mov edi,eax // Hook
            C1 E9 02                   - shr ecx,02 // Hook
            74 02                      - je 03C92270 // Hook
        F3 A5                      - repe movsd 
        B1 03                      - mov cl,03
        23 CA                      - and ecx,edx
        74 02                      - je 03C92278
        F3 A4                      - repe movsb 
        5F                         - pop edi
        5E                         - pop esi
        C3                         - ret 
                    */

                        // Free last hook
                        if (_codeCaveScanDump > 0)
                        {
                            Memory.WowMemory.Memory.FreeMemory(_codeCaveScanDump - StartDetourPtr);
                            _codeCaveScanDump = 0;
                        }
                        // Free last hook
                        if (_codeCaveDetourPtr > 0)
                        {
                            Memory.WowMemory.Memory.FreeMemory(_codeCaveDetourPtr - StartDetourPtr);
                            _codeCaveDetourPtr = 0;
                        }


                        // Alloc codecave
                        StartDetourPtr = (uint) Others.Random(5, 60);
                        _codeCaveDetourPtr = Memory.WowMemory.Memory.AllocateMemory((0xFA + Others.Random(60, 90))) +
                                             StartDetourPtr;
                        _codeCaveScanDump = Memory.WowMemory.Memory.AllocateMemory(0x4 + (0x8*100000));

                        if (_codeCaveDetourPtr - StartDetourPtr <= 0 || _codeCaveScanDump <= 0)
                        {
                            _codeCaveDetourPtr = 0;
                            StartDetourPtr = 0;
                            _codeCaveScanDump = 0;
                            return false;
                        }

                        // HOOK
                        Memory.WowMemory.Memory.Asm.Clear();

                        //Hook
                        int nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("pushfd");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("pushad");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("mov eax, [" + _codeCaveScanDump + "]");
                        // On met le pt dans eax
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("mov [eax],esi");
                        // On met l'adresse lit par le AccountSecurity dans eax
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("add eax, 4"); // On ajouter 4 a eax
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("test ebx, ebx"); // Test si long == 0 pour quitter
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("je @out");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("mov [eax],edx");
                        // On écrit la longueur lit par le AccountSecurity.
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("add eax, 4"); // On ajouter 4 a eax
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("mov [" + _codeCaveScanDump + "], eax");
                        // On met le nouveau pt pour le prochain dump
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("@out:");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("popad");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("popfd");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        // Original code
                        Memory.WowMemory.Memory.Asm.AddLine("mov ecx,edx");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("mov edi,eax");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("shr ecx,02");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }
                        // Return to AccountSecurityscan
                        Memory.WowMemory.Memory.Asm.AddLine("jmp " + (ScanFunction + 0xA));
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(MemoryClass.Hook.ProtectHook());
                        }

                        // Inject detour func
                        Memory.WowMemory.Memory.Asm.Inject(_codeCaveDetourPtr);

                        Memory.WowMemory.Memory.WriteUInt(_codeCaveScanDump, _codeCaveScanDump + 0x4);
                        CurrentAddressReadDump = _codeCaveScanDump + 0x4;


                        // JUMP
                        Memory.WowMemory.Memory.Asm.Clear();
                        //Hook
                        Memory.WowMemory.Memory.Asm.AddLine("jmp " + _codeCaveDetourPtr);
                        Memory.WowMemory.Memory.Asm.AddLine("nop");
                        Memory.WowMemory.Memory.Asm.AddLine("nop");
                        Memory.WowMemory.Memory.Asm.AddLine("nop");
                        Memory.WowMemory.Memory.Asm.AddLine("nop");
                        Memory.WowMemory.Memory.Asm.Inject(ScanFunction);

                        Memory.WowMemory.Memory.Asm.Clear();

                        //Logging.Write("AccountSecurity protection activated.");
                    }

                    if (_codeCaveScanDump == 0 || _codeCaveDetourPtr == 0)
                    {
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("HookAccountSecurity > hook(): " + exception);
                return false;
            }
        }

        internal static void DisposeHook()
        {
            try
            {
                if (AccountSecurityScanFonctionAddress() <= 0)
                    return;

                Memory.WowMemory.Memory.WriteBytes(ScanFunction,
                                                   new byte[] {0x8B, 0xCA, 0x8B, 0xF8, 0xC1, 0xE9, 0x02, 0x74, 0x02});

                // Free last hook
                if (_codeCaveScanDump > 0)
                {
                    Memory.WowMemory.Memory.FreeMemory(_codeCaveScanDump - StartDetourPtr);
                    _codeCaveScanDump = 0;
                }
                // Free last hook
                if (_codeCaveDetourPtr > 0)
                {
                    Memory.WowMemory.Memory.FreeMemory(_codeCaveDetourPtr);
                    _codeCaveDetourPtr = 0;
                }

                _accountSecurityThreadIsAlive = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("AccountSecurityHook > disposeHook(): " + exception);
            }
        }

        private static Timer _timeFindAddressAccountSecurity = new Timer(1000*10);
        private static int _lastProcessId = Memory.WowProcess.ProcessId;
        private static int _nbTest;

        private static uint AccountSecurityScanFonctionAddress()
        {
            try
            {
                if (_lastProcessId != Memory.WowProcess.ProcessId || Memory.WowProcess.ProcessId == 0)
                {
                    ScanFunction = 0;
                    _nbTest = 0;
                    _codeCaveDetourPtr = 0;
                    _codeCaveScanDump = 0;
                }

                if (ScanFunction > 0)
                {
                    if (Memory.WowMemory.Memory.ReadByte(ScanFunction) == 0xE9)
                    {
                        return ScanFunction;
                    }
                }
                if ((_nbTest >= 1 && _lastProcessId == Memory.WowProcess.ProcessId) || !Memory.WowMemory.ThreadHooked)
                {
                    return ScanFunction;
                }
                if (_timeFindAddressAccountSecurity.IsReady && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                {
                    Thread.Sleep(1000);
                    _nbTest++;
                    _timeFindAddressAccountSecurity = new Timer(1000*60);
                    _lastProcessId = Memory.WowProcess.ProcessId;

                    //const uint MaxAddress = 0x10000000;
                    //uint address = 0x01000000;

                    var timerFindAccountSecurity = new Timer(15*1000);

                    /*
                    do
                    {
                        var m = new MemoryClass.Usefuls.MEMORY_BASIC_INFORMATION();
                        try
                        {
                            MemoryClass.Usefuls.VirtualQueryEx(Memory.WowMemory.Memory.ProcessHandle, address, out m, Marshal.SizeOf(m));
                            if (m.AllocationBase != 0 && m.RegionSize > 0x1000)
                            {
                                /* ORIGINAL FUNCTION SCANAccountSecurity
                56                         - push esi
                57                         - push edi
                FC                         - cld 
                8B 54 24 14                - mov edx,[esp+14]
                8B 74 24 10                - mov esi,[esp+10]
                8B 44 24 0C                - mov eax,[esp+0C]
                    8B CA                      - mov ecx,edx // Hook
                    8B F8                      - mov edi,eax // Hook
                    C1 E9 02                   - shr ecx,02 // Hook
                    74 02                      - je 03C92270 // Hook
                F3 A5                      - repe movsd 
                B1 03                      - mov cl,03
                23 CA                      - and ecx,edx
                74 02                      - je 03C92278
                F3 A4                      - repe movsb 
                5F                         - pop edi
                5E                         - pop esi
                C3                         - ret 
                            */ /*
                                uint pattern = Memory.WowMemory.Memory.FindPattern((uint)m.AllocationBase, m.RegionSize, new byte[] { 0x8b, 0xca, 0x8b, 0xf8, 0xc1, 0xe9, 2, 0x74, 2 }, "xxxxxxxxx");
                                if (pattern != m.AllocationBase)
                                {
                                    //Log.AddLog("ScanAccountSecurity function found.");
                                    scanFunction = pattern - 0x13;
                                    return scanFunction;
                                }
                            }
                        }
                        catch
                        {
                        }
                        address = (uint)m.BaseAddress + (uint)m.RegionSize;
                        // Thread.Sleep(1);
                    } while (address <= MaxAddress && !timerFindAccountSecurity.IsReady);
                    */

                    var result =
                        MemoryClass.Usefuls.FindPattern(new byte[] {0x8b, 0xca, 0x8b, 0xf8, 0xc1, 0xe9, 2, 0x74, 2},
                                                        "xxxxxxxxx");
                    ScanFunction = (result != null) ? result.dwAddress : 0;
                }
            }
            catch (Exception exception)
            {
                ScanFunction = 0;
                Logging.WriteError("AccountSecurityScanFonctionAddress(): " + exception);
            }
            return ScanFunction;
        }

        internal static string GetSpecialAddressScan(DumpScan dumpScan)
        {
            try
            {
                // Verif AccountSecurityscan jump hook fonction
                if (GetSpecialAddressScan(dumpScan, AccountSecurityScanFonctionAddress(), 0x9))
                    return "AccountSecurityscan jump hook fonction";
                // Verif AccountSecurityscan hook fonction
                if (GetSpecialAddressScan(dumpScan, _codeCaveDetourPtr, 0xFA + StartDetourPtr))
                    return "AccountSecurityscan hook fonction";
                // Verif stockage dump scan AccountSecurity
                if (GetSpecialAddressScan(dumpScan, _codeCaveScanDump, 0x4 + (0x8*100)))
                    return "stockage dump scan wawa";
                // Vérif hook jump endscene
                if (GetSpecialAddressScan(dumpScan, Memory.WowMemory.JumpAddress, 0x5))
                    return "hook jump";
                // Vérif hook endscene
                if (GetSpecialAddressScan(dumpScan, Memory.WowMemory.InjectedCodeDetour, 0x100))
                    return "hook endscene";
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetSpecialAddressScan(DumpScan dumpScan): " + exception);
            }
            return "";
        }

        private static bool GetSpecialAddressScan(DumpScan scanned, uint addressAtVerif, uint addressAtVerifCount)
        {
            try
            {
                if (scanned.Address <= 0 || addressAtVerif <= 0)
                    return false;

                var usedAddress = new List<uint>();
                for (int i = (int) addressAtVerifCount - 1; i >= 0; i--)
                {
                    usedAddress.Add(addressAtVerif + (uint) i);
                }

                var scannedAddress = new List<uint>();
                for (int i = scanned.Length - 1; i >= 0; i--)
                {
                    scannedAddress.Add(scanned.Address + (uint) i);
                }

                foreach (var u in usedAddress)
                {
                    if (scannedAddress.Contains(u))
                        return true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError(
                    "GetSpecialAddressScan(DumpScan scanned, uint addressAtVerif, uint addressAtVerifCount): " +
                    exception);
            }
            return false;
        }
    }
}