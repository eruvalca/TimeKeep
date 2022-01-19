using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeKeep.Server.Services;
using TimeKeep.Shared.Models;

namespace TimeKeep.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HolidaysController : ControllerBase
    {
        private readonly HolidaysService _holidaysService;

        public HolidaysController(HolidaysService holidaysService)
        {
            _holidaysService = holidaysService;
        }

        [HttpGet("{year}")]
        public async Task<ActionResult<List<Holiday>>> GetHolidaysByYear(int year)
        {
            return Ok(await _holidaysService.GetHolidaysByYear(year));
        }
    }
}
