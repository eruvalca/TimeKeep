using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeKeep.Server.Services;
using TimeKeep.Shared.Models;

namespace TimeKeep.Server.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PTOEntriesController : ControllerBase
    {
        private readonly PTOEntriesService _ptoEntriesService;

        public PTOEntriesController(PTOEntriesService ptoEntriesService)
        {
            _ptoEntriesService = ptoEntriesService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<PTOEntry>>> GetPTOEntriesByUserId(string userId)
        {
            return Ok(await _ptoEntriesService.GetPTOEntriesByUserId(userId));
        }

        [HttpGet("vacationCarriedOver/{userId}")]
        public async Task<ActionResult<PTOEntry>> GetVacationCarriedOverEntryByUserId(string userId)
        {
            var result = await _ptoEntriesService.GetVacationCarriedOverEntryByUserId(userId);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PTOEntry>> GetPTOEntryById(int id)
        {
            return await _ptoEntriesService.GetPTOEntryById(id);
        }

        [HttpPost]
        public async Task<ActionResult<PTOEntry>> CreatePTOEntry(PTOEntry ptoEntry)
        {
            ptoEntry = await _ptoEntriesService.CreatePTOEntry(ptoEntry);

            return CreatedAtAction("GetPTOEntryById", new { id = ptoEntry.PTOEntryId }, ptoEntry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePTOEntry(int id, PTOEntry ptoEntry)
        {
            _ = await _ptoEntriesService.UpdatePTOEntry(ptoEntry);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePTOEntry(int id)
        {
            await _ptoEntriesService.DeletePTOEntry(id);
            return NoContent();
        }
    }
}
