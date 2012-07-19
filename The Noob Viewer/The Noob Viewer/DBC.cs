using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace meshDatabase.Database
{

    public class Record
    {
        public Record(DBC dbc)
        {
            Source = dbc;
        }

        public DBC Source { get; private set; }
        public List<int> Values = new List<int>();

        public int this[int index]
        {
            get
            {
                return Values[index];
            }
        }

        public float GetFloat(int index)
        {
            var data = BitConverter.GetBytes(Values[index]);
            return BitConverter.ToSingle(data, 0);
        }

        public string GetString(int index)
        {
            return Source.GetStringByOffset(Values[index]);
        }
    }

    public class DBC
    {
        public DBC(Stream src)
        {
            using (src)
            {
                using (var reader = new BinaryReader(src))
                {
                    var magic = reader.ReadBytes(4);
                    RecordCount = reader.ReadInt32();
                    Records = new List<Record>(RecordCount);
                    Fields = reader.ReadInt32();
                    RecordSize = reader.ReadInt32();
                    var stringBlockSize = reader.ReadInt32();

                    for (int i = 0; i < RecordCount; i++)
                    {
                        var rec = new Record(this);
                        Records.Add(rec);
                        int size = 0;
                        for (int f = 0; f < Fields; f++)
                        {
                            if (size + 4 > RecordSize)
                            {
                                IsFaulty = true;
                                break;
                            }
                            rec.Values.Add(reader.ReadInt32());
                            size += 4;
                        }
                    }

                    StringBlock = reader.ReadBytes(stringBlockSize);
                }
            }
        }

        public string GetStringByOffset(int offset)
        {
            int len = 0;
            for (int i = offset; i < StringBlock.Length; i++)
            {
                if (StringBlock[i] == 0x00)
                {
                    len = (i - offset);
                    break;
                }
            }
            return Encoding.UTF8.GetString(StringBlock, offset, len);
        }

        public Record GetRecordById(int id)
        {
            // we assume Id is index 0
            return Records.Where(r => r.Values[0] == id).FirstOrDefault();
        }

        public string Name { get; private set; }
        public List<Record> Records { get; private set; }
        public int RecordCount { get; private set; }
        public int Fields { get; private set; }
        public int RecordSize { get; private set; }
        public byte[] StringBlock { get; private set; }
        public bool IsFaulty { get; private set; }
    }

}