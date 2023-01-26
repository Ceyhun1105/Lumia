using Lumia.DbContextFiles;
using Lumia.Models;

namespace Lumia.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }
        public List<Setting> GetSettings()
        {
            return _context.Settings.ToList();
        }
    }
}
