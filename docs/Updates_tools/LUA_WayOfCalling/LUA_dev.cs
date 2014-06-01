

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr GetModuleHandle(string lpModuleName);
    [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
    private static IntPtr WriteProcessMemory { get; set; }
    private static byte[] lua_pcall_original = new byte[] { 0x55, 0x8B, 0xEC, 0x51, 0x51 }; // Original first bytes of lua_pcall, protection against new scan
    static Lua()
    {
    WriteProcessMemory = IntPtr.Zero;
    }
    public static List<string> GetReturnValues(string Lua, string LuaFile = "lua")
    {
    List<string> Result = new List<string>();
    if (WriteProcessMemory == IntPtr.Zero)
    {
    IntPtr Kernel32 = GetModuleHandle("kernel32.dll");
    if (Kernel32 == IntPtr.Zero)
    {
    Logger.WriteLine("Unable to find kernel32.dll !", Logger.LogType.Error);
    return Result;
    }
    WriteProcessMemory = GetProcAddress(Kernel32, "WriteProcessMemory");
    if (WriteProcessMemory == IntPtr.Zero)
    {
    Logger.WriteLine("Unable to find kernel32.dll!WriteProcessMemory !", Logger.LogType.Error);
    return Result;
    }
    }
    lock (Memory.Executor.ExecutionLock)
    {
    //-------------------------------------------------------------
    // Read lua_State
    //-------------------------------------------------------------
    uint s_Context = Memory.MMemory.Read<uint>(Memory.BaseAddress + (uint)Patchables.Offsets.Lua.s_context);
    //-------------------------------------------------------------
    // Build strings buffer
    //-------------------------------------------------------------
    Lua = "local t = {(function() " + Lua + " end)()}; for i,v in ipairs(t) do if type(v) == \"bool\" then if v then t[i] = \"1\" else t[i] = \"0\" end end end return unpack(t)";
    List<byte> bBuffer_Lua = new List<byte>();
    bBuffer_Lua.AddRange(Encoding.UTF8.GetBytes(Lua));
    bBuffer_Lua.Add(0x0);
    bBuffer_Lua.AddRange(Encoding.UTF8.GetBytes(LuaFile));
    bBuffer_Lua.Add(0x0);
    //-------------------------------------------------------------
    // Allocate memory
    //-------------------------------------------------------------
    uint codeCave__Lua = Memory.MMemory.AllocateMemory((uint)Lua.Length + (uint)LuaFile.Length + 2 + 5 + 1024);
    uint codeCave__LuaResult = codeCave__Lua + (uint)Lua.Length + (uint)LuaFile.Length + 2 + 5;
    uint codeCave__PCallOriginalBytes = codeCave__Lua + (uint)Lua.Length + (uint)LuaFile.Length + 2;
    uint codeCave__LuaNumResult = codeCave__LuaResult;
    uint codeCave__LuaTop = codeCave__LuaNumResult + 0x4;
    uint codeCave__CurrentLuaResult = codeCave__LuaNumResult + 0x8;
    codeCave__LuaResult = codeCave__LuaNumResult + 0xC;
    uint LuaAddress = codeCave__Lua;
    uint LuaFileAddress = codeCave__Lua + (uint)Lua.Length + 1;
    //-------------------------------------------------------------
    // Write lua_pcall original bytes
    //-------------------------------------------------------------
    MyWoW2.Memory.MMemory.WriteBytes(codeCave__PCallOriginalBytes, lua_pcall_original);
    //-------------------------------------------------------------
    // Write strings
    //-------------------------------------------------------------
    MyWoW2.Memory.MMemory.WriteBytes(codeCave__Lua, bBuffer_Lua.ToArray());
    //-------------------------------------------------------------
    // Call Lua__GetReturnValues
    //-------------------------------------------------------------
    string[] Asm = new string[]
    {
    //-------------------------------------------------------------
    // Write lua_pcall original bytes
    //-------------------------------------------------------------
    "push 0",
    "push 5",
    "push " + codeCave__PCallOriginalBytes,
    "push " + (Memory.BaseAddress + (uint)Patchables.Offsets.Functions.lua_pcall),
    "push 0xFFFFFFFF",
    "call " + ((uint)WriteProcessMemory),
    //-------------------------------------------------------------
    // Reset Lua Top
    //------------------------------------------------------------
    "push " + 0x0,
    "push " + s_Context,
    "call " + (Memory.BaseAddress + (uint)Patchables.Offsets.Functions.lua_settop), // settop
    "add esp, 0x8",
    //-------------------------------------------------------------
    // Reset the num of results
    //-------------------------------------------------------------
    "xor eax, eax",
    "mov [" + codeCave__LuaNumResult + "], eax",
    //-------------------------------------------------------------
    // LoadBuffer
    //-------------------------------------------------------------
    "push " + LuaFileAddress,
    "push " + Lua.Length,
    "push " + LuaAddress,
    "push " + s_Context,
    "call " + (Memory.BaseAddress + (uint)Patchables.Offsets.Functions.luaL_loadbuffer), // loadbuffer
    "add esp, 0x10",
    "test eax, eax",
    "jnz @out",
    //-------------------------------------------------------------
    // Store Top value
    //-------------------------------------------------------------
    "push " + s_Context,
    "call " + (Memory.BaseAddress + (uint)Patchables.Offsets.Functions.lua_gettop), // gettop
    "add esp, 0x4",
    "mov [" + codeCave__LuaTop + "], eax",
    //-------------------------------------------------------------
    // Call the Lua script
    //------------------------------------------------------------
    "push " + -2,
    "push " + -1,
    "push " + 0,
    "push " + s_Context,
    "call " + (Memory.BaseAddress + (uint)Patchables.Offsets.Functions.lua_pcall), // pcall
    "add esp, 0x10",
    "test eax, eax",
    "jnz @out",
    //-------------------------------------------------------------
    // Get Top
    //-------------------------------------------------------------
    "push " + s_Context,
    "call " + (Memory.BaseAddress + (uint)Patchables.Offsets.Functions.lua_gettop), // gettop
    "add esp, 0x4",
    //-------------------------------------------------------------
    // Store the num of results
    //-------------------------------------------------------------
    "mov [" + codeCave__LuaNumResult + "], eax",
    //-------------------------------------------------------------
    // Loop trough all results
    //-------------------------------------------------------------
    "xor edx, edx",
    "mov ecx, " + codeCave__LuaResult,
    "@LoopResults:",
    "cmp edx, eax",
    "jnb @LoopResultsEnd",
    "push eax",
    "push edx",
    "push ecx",
    "inc edx",
    "push " + 0,
    "push edx",
    "push " + s_Context,
    "call " + (Memory.BaseAddress + (uint)Patchables.Offsets.Functions.lua_tolstring), // tolstring
    "add esp, 0xC",
    "pop ecx",
    "mov [ecx], eax",
    "pop edx",
    "pop eax",
    "add ecx, 0x4",
    "mov [" + codeCave__CurrentLuaResult + "], ecx",
    "inc edx",
    "jmp @LoopResults",
    "@LoopResultsEnd:",
    "jmp @out;",
    "@out:",
    "retn",
    };
    uint resultPtr = Memory.Executor.Execute(Asm, "GetReturnValues");
    int NumResults = Memory.MMemory.Read<int>(codeCave__LuaNumResult);
    for (int i = 0; i < NumResults; i++)
    {
    uint resultStrPtr = Memory.MMemory.Read<uint>(codeCave__LuaResult + (uint)i * 4);
    if (resultStrPtr != 0)
    {
    Result.Add(Memory.MMemory.ReadString(resultStrPtr, Encoding.UTF8));
    }
    else
    {
    Result.Add("0");
    }
    }
    Memory.MMemory.FreeMemory(codeCave__Lua);
    }
    return Result;
    }

