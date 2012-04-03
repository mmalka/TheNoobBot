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
    class HookAccountSecurity
    {
        #region address
        private static uint codeCave_DetourPtr;
        private static uint codeCave_ScanDump;
        internal static uint scanFunction;
        internal static uint currentAddressReadDump;
        internal static uint startDetourPtr;
        #endregion

        [StructLayout(LayoutKind.Explicit, Size = 0x8)]
        public struct DumpScan
        {
            [FieldOffset(0x0)]
            public uint Address;
            [FieldOffset(0x4)]
            public int Length;
        }

        static bool accountSecurityThreadIsAlive;
        internal static void Pulse()
        {
            try
            {
                lock (typeof(HookAccountSecurity))
                {
                    if (!accountSecurityThreadIsAlive)
                    {
                        accountSecurityThreadIsAlive = true;
                        var checkUpdateThreadLaunch = new Thread(loopAccountSecurityThread) { Name = "loopAccountSecurity" };
                        checkUpdateThreadLaunch.Start();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("HookAccountSecurity > Pulse(): " + exception);
            }
        }

        static void loopAccountSecurityThread()
        {
            try
            {
                while (true)
                {
                    if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting && Memory.WowMemory.ThreadHooked)
                    {
                        if (hook())
                        {
                            if (Memory.WowMemory.Memory.ReadUInt(codeCave_ScanDump) > currentAddressReadDump)
                            {
                                var dumpScanTemps = (DumpScan)Memory.WowMemory.Memory.ReadObject(currentAddressReadDump, typeof(DumpScan));
                                if (dumpScanTemps.Length > 0)
                                {
                                    var specialAddress = GetSpecialAddressScan(dumpScanTemps);
                                    if (specialAddress != "")
                                    {
                                        Memory.WowProcess.KillWowProcess();
                                        Others.GetRequest("http://tech.thenoobbot.com/newScanAccountSecurity.php",
                                                          "msg=" + specialAddress);
                                        Logging.Write("Suspect activity at this adresse: " + specialAddress, Logging.LogType.Normal, Color.Red);
                                        AccountSecurity.DialogNewScan();
                                    }
                                }

                                dumpScanTemps.Length = 0;
                                dumpScanTemps.Address = 0;
                                Memory.WowMemory.Memory.WriteObject(currentAddressReadDump, dumpScanTemps);

                                currentAddressReadDump = currentAddressReadDump + (uint)Marshal.SizeOf(typeof(DumpScan));
                            }
                            else
                            {
                                if (Memory.WowMemory.Memory.ReadUInt(codeCave_ScanDump) > codeCave_ScanDump + 0x4 + (0x7 * 100000))
                                {
                                    Memory.WowMemory.Memory.WriteUInt(codeCave_ScanDump, codeCave_ScanDump + 0x4);
                                    Thread.Sleep(10);
                                    if (Memory.WowMemory.Memory.ReadUInt(codeCave_ScanDump) < codeCave_ScanDump)
                                    {
                                        currentAddressReadDump = codeCave_ScanDump + 0x4;
                                    }
                                }
                            }
                        }
                    }
                    Thread.Sleep(350);//150
                }
            }
            catch (Exception exception)
            {
                accountSecurityThreadIsAlive = false;
                Logging.WriteError("loopAccountSecurityThread(): " + exception);
            }
            // ReSharper disable FunctionNeverReturns
        }
        // ReSharper restore FunctionNeverReturns

        static bool hook()
        {

            try
            {
                if (!Memory.WowMemory.ThreadHooked)
                    return false;

                if (AccountSecurityScanFonctionAddress() <= 0)
                    return false;

                lock (Hook.Locker)
                {
                    if (Memory.WowMemory.Memory.ReadByte(scanFunction) == 0x8B || (Memory.WowMemory.Memory.ReadByte(scanFunction) == 0xE9 && codeCave_ScanDump <= 0 && codeCave_DetourPtr <= 0))
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
                        if (codeCave_ScanDump > 0)
                        {
                            Memory.WowMemory.Memory.FreeMemory(codeCave_ScanDump - startDetourPtr);
                            codeCave_ScanDump = 0;
                        }
                        // Free last hook
                        if (codeCave_DetourPtr > 0)
                        {
                            Memory.WowMemory.Memory.FreeMemory(codeCave_DetourPtr - startDetourPtr);
                            codeCave_DetourPtr = 0;
                        }


                        // Alloc codecave
                        startDetourPtr = (uint)Others.Random(5, 60);
                        codeCave_DetourPtr = Memory.WowMemory.Memory.AllocateMemory((0xFA + Others.Random(60, 90))) + startDetourPtr;
                        codeCave_ScanDump = Memory.WowMemory.Memory.AllocateMemory(0x4 + (0x8 * 100000));

                        if (codeCave_DetourPtr - startDetourPtr <= 0 || codeCave_ScanDump <= 0)
                        {
                            codeCave_DetourPtr = 0;
                            startDetourPtr = 0;
                            codeCave_ScanDump = 0;
                            return false;
                        }

                        // HOOK
                        Memory.WowMemory.Memory.Asm.Clear();

                        //Hook
                        int nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("pushfd");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("pushad");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("mov eax, [" + codeCave_ScanDump + "]"); // On met le pt dans eax
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("mov [eax],esi"); // On met l'adresse lit par le AccountSecurity dans eax
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("add eax, 4"); // On ajouter 4 a eax
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("test ebx, ebx"); // Test si long == 0 pour quitter
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("je @out");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("mov [eax],edx"); // On écrit la longueur lit par le AccountSecurity.
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("add eax, 4"); // On ajouter 4 a eax
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("mov [" + codeCave_ScanDump + "], eax"); // On met le nouveau pt pour le prochain dump
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("@out:");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("popad");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("popfd");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        // Original code
                        Memory.WowMemory.Memory.Asm.AddLine("mov ecx,edx");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("mov edi,eax");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        Memory.WowMemory.Memory.Asm.AddLine("shr ecx,02");
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }
                        // Return to AccountSecurityscan
                        Memory.WowMemory.Memory.Asm.AddLine("jmp " + (scanFunction + 0xA));
                        nR = Others.Random(1, 3);
                        for (int i = nR; i >= 1; i--)
                        {
                            Memory.WowMemory.Memory.Asm.AddLine(Hook.ProtectHook());
                        }

                        // Inject detour func
                        Memory.WowMemory.Memory.Asm.Inject(codeCave_DetourPtr);

                        Memory.WowMemory.Memory.WriteUInt(codeCave_ScanDump, codeCave_ScanDump + 0x4);
                        currentAddressReadDump = codeCave_ScanDump + 0x4;


                        // JUMP
                        Memory.WowMemory.Memory.Asm.Clear();
                        //Hook
                        Memory.WowMemory.Memory.Asm.AddLine("jmp " + codeCave_DetourPtr);
                        Memory.WowMemory.Memory.Asm.AddLine("nop");
                        Memory.WowMemory.Memory.Asm.AddLine("nop");
                        Memory.WowMemory.Memory.Asm.AddLine("nop");
                        Memory.WowMemory.Memory.Asm.AddLine("nop");
                        Memory.WowMemory.Memory.Asm.Inject(scanFunction);

                        Memory.WowMemory.Memory.Asm.Clear();

                        //Logging.Write("AccountSecurity protection activated.");
                    }

                    if (codeCave_ScanDump == 0 || codeCave_DetourPtr == 0)
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

        internal static void disposeHook()
        {
            try
            {
                if (AccountSecurityScanFonctionAddress() <= 0)
                    return;

                Memory.WowMemory.Memory.WriteBytes(scanFunction, new byte[] { 0x8B, 0xCA, 0x8B, 0xF8, 0xC1, 0xE9, 0x02, 0x74, 0x02 });

                // Free last hook
                if (codeCave_ScanDump > 0)
                {
                    Memory.WowMemory.Memory.FreeMemory(codeCave_ScanDump - startDetourPtr);
                    codeCave_ScanDump = 0;
                }
                // Free last hook
                if (codeCave_DetourPtr > 0)
                {
                    Memory.WowMemory.Memory.FreeMemory(codeCave_DetourPtr);
                    codeCave_DetourPtr = 0;
                }

                accountSecurityThreadIsAlive = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("AccountSecurityHook > disposeHook(): " + exception);
            }
        }

        static Timer timeFindAddressAccountSecurity = new Timer(1000 * 10);
        static int lastProcessId = Memory.WowProcess.ProcessId;
        static int nbTest;
        static uint AccountSecurityScanFonctionAddress()
        {
            try
            {
                if (lastProcessId != Memory.WowProcess.ProcessId || Memory.WowProcess.ProcessId == 0)
                {
                    scanFunction = 0;
                    nbTest = 0;
                    codeCave_DetourPtr = 0;
                    codeCave_ScanDump = 0;
                }

                if (scanFunction > 0)
                {
                    if (Memory.WowMemory.Memory.ReadByte(scanFunction) == 0xE9)
                    {
                        return scanFunction;
                    }
                }
                if ((nbTest >= 1 && lastProcessId == Memory.WowProcess.ProcessId) || !Memory.WowMemory.ThreadHooked)
                {
                    return scanFunction;
                }
                if (timeFindAddressAccountSecurity.IsReady && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                {
                    Thread.Sleep(1000);
                    nbTest++;
                    timeFindAddressAccountSecurity = new Timer(1000 * 60);
                    lastProcessId = Memory.WowProcess.ProcessId;

                    const uint MaxAddress = 0x10000000;
                    uint address = 0x01000000;

                    var timerFindAccountSecurity = new Timer(15 * 1000);

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
                            *//*
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

                    var result = MemoryClass.Usefuls.FindPattern(new byte[] {0x8b, 0xca, 0x8b, 0xf8, 0xc1, 0xe9, 2, 0x74, 2}, "xxxxxxxxx");
                    if ((result != null))
                        scanFunction = result.dwAddress;
                    else
                        scanFunction = 0;
                }
            }
            catch (Exception exception)
            {
                scanFunction = 0;
                Logging.WriteError("AccountSecurityScanFonctionAddress(): " + exception);
            }
            return scanFunction;
        }

        internal static string GetSpecialAddressScan(DumpScan dumpScan)
        {
            try
            {
                // Verif AccountSecurityscan jump hook fonction
                if (GetSpecialAddressScan(dumpScan, AccountSecurityScanFonctionAddress(), 0x9))
                    return "AccountSecurityscan jump hook fonction";
                // Verif AccountSecurityscan hook fonction
                if (GetSpecialAddressScan(dumpScan, codeCave_DetourPtr, 0xFA + startDetourPtr))
                    return "AccountSecurityscan hook fonction";
                // Verif stockage dump scan AccountSecurity
                if (GetSpecialAddressScan(dumpScan, codeCave_ScanDump, 0x4 + (0x8 * 100)))
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

        static bool GetSpecialAddressScan(DumpScan scanned, uint addressAtVerif, uint addressAtVerifCount)
        {
            try
            {
                if (scanned.Address <= 0 || addressAtVerif <= 0)
                    return false;

                var usedAddress = new List<uint>();
                for (int i = (int)addressAtVerifCount - 1; i >= 0; i--)
                {
                    usedAddress.Add(addressAtVerif + (uint)i);
                }

                var scannedAddress = new List<uint>();
                for (int i = scanned.Length - 1; i >= 0; i--)
                {
                    scannedAddress.Add(scanned.Address + (uint)i);
                }

                foreach (var u in usedAddress)
                {
                    if (scannedAddress.Contains(u))
                        return true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetSpecialAddressScan(DumpScan scanned, uint addressAtVerif, uint addressAtVerifCount): " + exception);
            }
            return false;
        }
    }


}
