using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeKeep.Server.Services;

namespace TimeKeep.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly PTOEntriesService _ptoEntriesService;

        public AvailabilityController(PTOEntriesService ptoEntriesService)
        {
            _ptoEntriesService = ptoEntriesService;
        }

        [HttpGet]
        public async Task<ActionResult<int>> GetPTOEntriesCount()
        {
            return await _ptoEntriesService.GetPTOEntriesCount();
        }
    }
}
