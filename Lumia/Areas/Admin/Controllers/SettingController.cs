using Lumia.DbContextFiles;
using Lumia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lumia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Setting> settings = _context.Settings.ToList();
            return View(settings);
        }

        public IActionResult Update(int id)
        {
            Setting setting = _context.Settings.FirstOrDefault(x => x.Id == id);

            if (setting == null) return NotFound();

            return View(setting);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Setting setting)
        {
            Setting existsetting = _context.Settings.FirstOrDefault(x => x.Id == setting.Id);
            if(existsetting == null) return NotFound();

            if (!ModelState.IsValid) return View(setting);

            existsetting.Value = setting.Value;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
