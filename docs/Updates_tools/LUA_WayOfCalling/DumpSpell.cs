Memory.WowMemory.LockFrame();
            var lCache = new Dictionary<int, string>();
            var fileName = "spellList.txt";
            var from = 0;
            var to = 300000;
            var spellDb2 = Memory.WowMemory.Memory.RebaseAddress(0xF35758);
            var DB2Context = Memory.WowMemory.Memory.RebaseAddress(0xD20E70);
            var DB2Reader = Memory.WowMemory.Memory.RebaseAddress(0x20CBC3);
            var ret = Memory.WowMemory.Memory.AllocateMemory(4);
            for (int i = 0; i < 300000; i++)
            {
                Memory.WowMemory.InjectAndExecute(new []
                {
                    "mov ecx, " + DB2Context,
                    Memory.WowMemory.CallWrapperCode(DB2Reader, i, spellDb2, 0, 0, 0),
                    "mov edi, " + ret,
                    "mov [edi], eax",
                    Memory.WowMemory.RetnToHookCode
                });
                var result = Memory.WowMemory.Memory.ReadUInt32(ret);
                if (result > 0)
                {
                    var name = Memory.WowMemory.Memory.ReadStringUTF8(result + Memory.WowMemory.Memory.ReadUInt32(result), 100);
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        lCache.Add(i, name);
                    }
                }
            }
            using (var f = new StreamWriter(fileName, false))
            {
                for (var i = from; i <= to; i++)
                {
                    if (!lCache.ContainsKey(i))
                        continue;
                    f.WriteLine(i + ";" + lCache[i]);
                }
            }
            GZip.Compress(fileName, fileName + ".gz");
        }
        finally
        {
            Memory.WowMemory.UnlockFrame();
        }