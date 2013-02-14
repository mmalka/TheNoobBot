namespace meshDatabase.Database
{

    public class TaxiPath
    {
        public int Id { get; private set; }
        public int From { get; private set; }
        public int To { get; private set; }
        public int Cost { get; private set; }

        public bool IsValid
        {
            get { return From > 0 && To > 0; }
        }

        public TaxiPath(Record rec)
        {
            Id = rec[0];
            From = rec[1];
            To = rec[2];
            Cost = rec[3];
        }
    }

}