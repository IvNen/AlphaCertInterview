using CanWeFixItService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CanWeFixItApi.Controllers
{
    [Route("v1/valuations")]
    [ApiController]
    public class ValuationsController : ControllerBase
    {
        private readonly IDatabaseService _database;

        public ValuationsController(IDatabaseService database)
        {
            _database = database;
        }

        public async Task<ActionResult<MarketValuation>> Get()
        {
            return Ok(await _database.MarketValuation());
        }
    }
}
