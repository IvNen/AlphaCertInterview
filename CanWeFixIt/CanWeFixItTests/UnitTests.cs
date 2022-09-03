using CanWeFixItService;
using Microsoft.Data.Sqlite;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace CanWeFixItTests
{
    public class UnitTests
    {
        const string connectionString = "Data Source=DatabaseService;Mode=Memory";
        private readonly DatabaseService databaseService;

        public UnitTests()
        {
            databaseService = new DatabaseService(connectionString);
            databaseService.SetupDatabase();
        }

        [Fact]
        public void MarketDataTest()
        {
            var data = databaseService.MarketData().Result;

            // Check correct type of object is returned
            Assert.IsType<MarketDataDto>(data.FirstOrDefault());

            // Check count returned is 2 
            Assert.Equal(2, data.Count());

            foreach (var item in data)
            {
                // Check each item is active in the collection
                Assert.True(item.Active);

                // Check id is either 2 or 4
                Assert.InRange(item.Id, 2, 4);

                // Check instrumentId is either 2 or 4
                Assert.InRange(item.InstrumentId.Value, 2, 4);
            }


        }

        [Fact]
        public void InstrumentTest()
        {
            var data = databaseService.Instruments().Result;

            //Check correct type is returned
            Assert.IsType<Instrument>(data.FirstOrDefault());

            // Check count returned is 4
            Assert.Equal(4, data.Count());

            foreach (var item in data)
            {
                // Check each item is active in the collection
                Assert.True(item.Active);

                // Check id is either in range 2-8 
                Assert.InRange(item.Id, 2, 8);
            }
        }

        [Fact]
        public void MarketValuationTest()
        {
            var marketValuationData = databaseService.MarketValuation().Result;

            // Check is the correct content type
            Assert.IsType<MarketValuation>(marketValuationData.FirstOrDefault());

            // Check only one value returned in the list
            Assert.Single(marketValuationData);

            // Check total returned
            Assert.Equal(13332, marketValuationData.FirstOrDefault().Total);

            // Check name field is correct
            Assert.Equal("DataValueTotal", marketValuationData.FirstOrDefault().Name);
        }
    }
}
