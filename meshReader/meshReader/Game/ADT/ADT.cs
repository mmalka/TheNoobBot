using System.IO;
using System.Linq;

namespace meshReader.Game.ADT
{

    public class ADT
    {
        public ChunkedData ObjectData { get; private set; }
        public ChunkedData Data { get; private set; }
        public MapChunk[] MapChunks { get; private set; }
        public MHDR Header { get; private set; }
        public bool HasObjectData { get; private set; }

        public DoodadHandler DoodadHandler { get; private set; }
        public WorldModelHandler WorldModelHandler { get; private set; }
        public LiquidHandler LiquidHandler { get; private set; }

        public ADT(string file)
        {
            try
            {
                Data = new ChunkedData(file);
                ObjectData = new ChunkedData(file.Replace(".adt", "_obj0.adt"));
                HasObjectData = true;
            }
            catch (FileNotFoundException)
            {
                ObjectData = null;
                HasObjectData = false;
            }
        }

        public void Read()
        {
            Header = new MHDR();
            Header.Read(Data.GetChunkByName("MHDR").GetStream());

            MapChunks = new MapChunk[16 * 16];
            int mapChunkIndex = 0;
            foreach (var mapChunk in Data.Chunks.Where(c => c.Name == "MCNK"))
                MapChunks[mapChunkIndex++] = new MapChunk(this, mapChunk);

            LiquidHandler = new LiquidHandler(this);

            // do this seperate from map chunk initialization to access liquid data
            foreach (var mapChunk in MapChunks)
                mapChunk.GenerateTriangles();

            DoodadHandler = new DoodadHandler(this);
            foreach (var mapChunk in MapChunks)
                DoodadHandler.ProcessMapChunk(mapChunk);

            WorldModelHandler = new WorldModelHandler(this);
            foreach (var mapChunk in MapChunks)
                WorldModelHandler.ProcessMapChunk(mapChunk);
        }
    }

}