namespace meshDatabase.Database
{
    class FileDataEntry
    {
        public int DataId { get; private set; }
        public string FileName { get; private set; }
        public string FilePath { get; private set; }

        public FileDataEntry(Record rec)
        {
            DataId = rec[0];
            FileName = rec.GetString(1);
            FilePath = rec.GetString(2);
        }
    }
}