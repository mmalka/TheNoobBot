namespace meshReader.Game
{
    public static class Constant
    {
        public const int Division = 2;
        public const float BaseTileSize = 533f + (1 / (float) 3);
        public const float TileSize = BaseTileSize / Division;
        public const float MaxXY = 32.0f * BaseTileSize;
        public const float ChunkSize = BaseTileSize / 16.0f;
        public const float UnitSize = ChunkSize / 8.0f;
    }
}