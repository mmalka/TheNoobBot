using System.IO;
using System.Text;
using meshReader.Game.ADT;
using meshReader.Helper;

namespace meshReader.Game
{
    
    public class WDT
    {
        public ChunkedData Data { get; private set; }
        public bool[,] TileTable { get; private set; }
        public bool IsGlobalModel { get; private set; }
        public bool IsValid;

        public string ModelFile { get; private set; }
        public WorldModelHandler.WorldModelDefinition ModelDefinition { get; private set; }

        public WDT(string file)
        {
            Data = new ChunkedData(file, 2);
            ReadTileTable();
            ReadGlobalModel();
        }

        private void ReadGlobalModel()
        {
            var fileChunk = Data.GetChunkByName("MWMO");
            var defChunk = Data.GetChunkByName("MODF");// this is null always
            if (fileChunk == null || defChunk == null)// so this, returns...
                return;

            IsGlobalModel = true;// IsGLobalModel needs to be true... butwhen i do step by step

            ModelDefinition = WorldModelHandler.WorldModelDefinition.Read(defChunk.GetStream());
            ModelFile = fileChunk.GetStream().ReadCString();
        }

        private void ReadTileTable()
        {
            var chunk = Data.GetChunkByName("MAIN");
            if (chunk == null)
                return;

            IsValid = true;
            TileTable = new bool[64,64];
            var r = new BinaryReader(chunk.GetStream());
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    const int hasTileFlag = 0x1;
                    var flags = r.ReadUInt32();
                    r.ReadUInt32();
                    if ((flags & hasTileFlag) > 0)
                        TileTable[x, y] = true;
                    else
                        TileTable[x, y] = false;
                }
            }
        }

        public bool HasTile(int x, int y)
        {
            return TileTable[x, y];
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                    if (HasTile(x, y))
                        builder.Append("X");
                    else
                        builder.Append("_");
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }

}