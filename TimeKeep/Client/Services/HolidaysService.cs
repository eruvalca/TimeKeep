using System.Net.Http.Json;
using TimeKeep.Shared.Models;

namespace TimeKeep.Client.Services
{
    public class HolidaysService
    {
        private readonly HttpClient _client;

        public HolidaysService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<Holiday>> GetHolidaysByYear(int year)
        {
            return await _client.GetFromJsonAsync<List<Holiday>>($"holidays/{year}");
        }
    }
}
