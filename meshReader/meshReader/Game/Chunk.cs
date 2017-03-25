using System.IO;
using System.Text;
using System.Linq;

namespace meshReader.Game
{
    public class Chunk
    {
        private readonly Stream _sharedStream;

        public string Name { get; private set; }
        public uint Length { get; private set; }
        public uint Offset { get; private set; }

        public Chunk(string name, uint length, uint offset, Stream sharedStream)
        {
            _sharedStream = sharedStream;
            Name = name;
            Length = length;
            Offset = offset;
        }

        public int FindSubChunkOffset(string name)
        {
            var bytes = Encoding.ASCII.GetBytes(name).Reverse().ToArray();
            if (bytes.Length != 4)
                return -1;

            var stream = GetStream();
            int matched = 0;
            while (stream.Position < stream.Length)
            {
                var b = stream.ReadByte();
                if (b == bytes[matched])
                    matched++;
                else
                    matched = 0;
                if (matched == 4)
                    return (int) (stream.Position - 4);
            }
            return -1;
        }

        public Stream GetStream()
        {
            _sharedStream.Seek(Offset, SeekOrigin.Begin);
            return _sharedStream;
        }
    }
}