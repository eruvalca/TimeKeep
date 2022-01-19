using Microsoft.EntityFrameworkCore;
using TimeKeep.Server.Data;
using TimeKeep.Shared.Models;

namespace TimeKeep.Server.Services
{
    public class HolidaysService
    {
        private readonly AppDbContext _context;

        public HolidaysService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Holiday>> GetHolidaysByYear(int year)
        {
            return await _context.Holidays
                .Where(p => p.Year == year)
                .ToListAsync();
        }
    }
}
