namespace CanWeFixItService
{
    public class MarketData
    {
        public int Id { get; set; }
        public long? DataValue { get; set; }
        public int? InstrumentId { get; set; }
        public bool Active { get; set; }
    }
}