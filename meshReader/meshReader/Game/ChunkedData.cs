using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using meshDatabase;

namespace meshReader.Game
{

    public class ChunkedData
    {
        public Stream Stream { get; private set; }
        public List<Chunk> Chunks { get; private set; }

        public ChunkedData(Stream stream, int maxLength, int chunksHint = 300)
        {
            Stream = stream;
            if (maxLength == 0)
                maxLength = (int)stream.Length;
            Chunks = new List<Chunk>(chunksHint);
            var reader = new BinaryReader(stream);
            var baseOffset = (uint) stream.Position;
            uint calcOffset = 0;
            while ((calcOffset+baseOffset) < stream.Length && (calcOffset < maxLength))
            {
                //try
                //{
                    var nameBytes = reader.ReadBytes(4);
                    var name = Encoding.ASCII.GetString(new[] { nameBytes[3], nameBytes[2], nameBytes[1], nameBytes[0] });
                    uint length = 0;
                    try
                    {
                        length = reader.ReadUInt32();
                        calcOffset += 8;
                    }
                    catch
                    {
                        calcOffset += 6;
                    }
                    Chunks.Add(new Chunk(name, length, calcOffset + baseOffset, Stream));
                    calcOffset += length;
                    // save an extra seek at the end
                    if ((calcOffset+baseOffset) < stream.Length && calcOffset < maxLength)
                        stream.Seek(length, SeekOrigin.Current);
                //}
                //catch { return; }
            }
        }

        public ChunkedData(string file, int chunkHint = 300)
            : this(MpqManager.GetFile(file), 0, chunkHint)
        {
        }

        public int GetFirstIndex(string chunkName)
        {
            for (int i = 0; i < Chunks.Count; i++)
                if (Chunks[i].Name == chunkName)
                    return i;
            return -1;
        }

        public Chunk GetChunkByName(string name)
        {
            return Chunks.Where(c => c.Name == name).FirstOrDefault();
        }
    }

}