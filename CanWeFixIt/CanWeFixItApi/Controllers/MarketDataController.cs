using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CanWeFixItService;

namespace CanWeFixItApi.Controllers
{
    [ApiController]
    [Route("v1/marketdata")]
    public class MarketDataController : ControllerBase
    {
        private readonly IDatabaseService _database;

        public MarketDataController(IDatabaseService database)
        {
            _database = database;
        }

        // GET
        public async Task<ActionResult<IEnumerable<MarketData>>> Get()
        {
            var data = await _database.MarketData();
            return Ok(await _database.MarketData());
        }
    }
}