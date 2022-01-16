using TimeKeep.Server.Data;
using TimeKeep.Shared.Models;
using TimeKeep.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace TimeKeep.Server.Services
{
    public class PTOEntriesService
    {
        private readonly AppDbContext _context;

        public PTOEntriesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetPTOEntriesCount()
        {
            return await _context.PTOEntries.CountAsync();
        }

        public async Task<List<PTOEntry>> GetPTOEntriesByUserId(string userId)
        {
            return await _context.PTOEntries
                .Where(p => p.TimeKeepUserId == userId
                    && p.PTODate.Year == DateTime.Today.Year)
                .ToListAsync();
        }

        public async Task<PTOEntry> GetVacationCarriedOverEntryByUserId(string userId)
        {
            return await _context.PTOEntries
                .Where(p => p.PTOType == PTOType.VacationCarriedOver
                    && p.TimeKeepUserId == userId
                    && p.PTODate.Year == DateTime.Today.Year)
                .OrderByDescending(p => p.PTOEntryId)
                .FirstOrDefaultAsync();
        }

        public async Task<PTOEntry> GetPTOEntryById(int id)
        {
            return await _context.PTOEntries
                .Where(p => p.PTOEntryId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<PTOEntry> CreatePTOEntry(PTOEntry ptoEntry)
        {
            ptoEntry.PTOHours *= -1;
            ptoEntry.PTODate = ptoEntry.PTODate.ToUniversalTime();
            ptoEntry.CreateDate = ptoEntry.CreateDate.ToUniversalTime();

            await _context.PTOEntries.AddAsync(ptoEntry);
            await _context.SaveChangesAsync();

            return ptoEntry;
        }

        public async Task<PTOEntry> UpdatePTOEntry(PTOEntry ptoEntry)
        {
            ptoEntry.PTOHours *= -1;
            ptoEntry.PTODate = ptoEntry.PTODate.ToUniversalTime();
            ptoEntry.ModifyDate = ptoEntry.ModifyDate?.ToUniversalTime();

            _context.Entry(ptoEntry).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return ptoEntry;
        }

        public async Task DeletePTOEntry(int id)
        {
            var ptoEntry = await _context.PTOEntries
                .Where(p => p.PTOEntryId == id)
                .FirstOrDefaultAsync();

            _context.PTOEntries.Remove(ptoEntry);
            await _context.SaveChangesAsync();
        }
    }
}
