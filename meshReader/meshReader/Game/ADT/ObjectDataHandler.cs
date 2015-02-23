namespace meshReader.Game.ADT
{
    
    public abstract class ObjectDataHandler
    {
        public ADT Source { get; private set; }

        protected ObjectDataHandler(ADT adtFile)
        {
            Source = adtFile;
        }

        public void ProcessMapChunk(MapChunk chunk)
        {
            if (!Source.HasObjectData)
                return;

            // fuck it blizzard, why is this crap necessary?
            var firstIndex = Source.ObjectData.GetFirstIndex("MCNK");
            if (firstIndex == -1)
                return;
            if (firstIndex + chunk.Index >= Source.ObjectData.Chunks.Count)
                return;
            var ourChunk = Source.ObjectData.Chunks[firstIndex + chunk.Index];
            if (ourChunk.Length == 0)
                return;
            var subChunks = new ChunkedData(ourChunk.GetStream(), (int)ourChunk.Length, 2);
            ProcessInternal(subChunks);
        }

        protected abstract void ProcessInternal(ChunkedData subChunks);
    }

}