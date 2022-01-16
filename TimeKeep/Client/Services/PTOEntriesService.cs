using System.Net;
using System.Net.Http.Json;
using TimeKeep.Shared.Models;

namespace TimeKeep.Client.Services
{
    public class PTOEntriesService
    {
        private readonly HttpClient _client;

        public PTOEntriesService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<PTOEntry>> GetPTOEntriesByUserId(string userId)
        {
            return await _client.GetFromJsonAsync<List<PTOEntry>>($"ptoEntries/user/{userId}");
        }

        public async Task<PTOEntry> GetVacationCarriedOverEntryByUserId(string userId)
        {
            var result = await _client.GetAsync($"ptoEntries/vacationCarriedOver/{userId}");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return await result.Content.ReadFromJsonAsync<PTOEntry>(); ;
            }

            return null;
        }

        public async Task<PTOEntry> GetPTOEntryById(int id)
        {
            return await _client.GetFromJsonAsync<PTOEntry>($"ptoEntries/{id}");
        }

        public async Task<PTOEntry> CreatePTOEntry(PTOEntry ptoEntry)
        {
            var response = await _client.PostAsJsonAsync("ptoEntries", ptoEntry);
            return await response.Content.ReadFromJsonAsync<PTOEntry>();
        }

        public async Task UpdatePTOEntry(int id, PTOEntry ptoEntry)
        {
            await _client.PutAsJsonAsync($"ptoEntries/{id}", ptoEntry);
        }

        public async Task DeletePTOEntry(int id)
        {
            await _client.DeleteAsync($"ptoEntries/{id}");
        }
    }
}
