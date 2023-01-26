using Lumia.DbContextFiles;
using Lumia.Helpers;
using Lumia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Lumia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Teams.Include(x=>x.Position).AsQueryable<Team>();
            var list = PaginatedList<Team>.Create(query, 2, page);
            /*List<Team> teams = _context.Teams.Include(x => x.Position).ToList();*/
            return View(list);
        }
        public IActionResult Create()
        {
            ViewBag.Positions = _context.Positions.ToList();
            return View();
        }
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult Create(Team team)
        {
            ViewBag.Positions = _context.Positions.ToList();
           

            if (!ModelState.IsValid) return View(team);

            if(team.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "Required");
                return View(team);
            }

            if(!team.ImageFile.CheckFileLength(1048576*3))
            {
                ModelState.AddModelError("ImageFile", "Please,upload less than 3 Mb");
                return View(team);
            }
            if (!team.ImageFile.CheckFileType())
            {
                ModelState.AddModelError("ImageFile", "Please,upload only jpeg, jpg and png files");
                return View(team);
            }

            team.ImageUrl = team.ImageFile.SaveFile(_env.WebRootPath, "uploads/teams");

            _context.Teams.Add(team);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));

        }
        public IActionResult Update(int id)
        {
            var team = _context.Teams.Include(x=>x.Position).FirstOrDefault(x => x.Id == id);
            if (team == null) return NotFound();
            ViewBag.Positions = _context.Positions.ToList();
            ViewBag.Image = team.ImageUrl;

            return View(team);
            
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        
        public IActionResult Update(Team team)
        {
            Team existteam = _context.Teams.Include(x => x.Position).FirstOrDefault(x => x.Id == team.Id);
            if(existteam == null) return NotFound();
            ViewBag.Positions = _context.Positions.ToList();
            ViewBag.Image = existteam.ImageUrl;

            if (!ModelState.IsValid) return View(team);

            if(team.ImageFile is not null)
            {
                if (!team.ImageFile.CheckFileLength(1048576 * 3))
                {
                    ModelState.AddModelError("ImageFile", "Please,upload less than 3 Mb");
                    return View(team);
                }
                if (!team.ImageFile.CheckFileType())
                {
                    ModelState.AddModelError("ImageFile", "Please,upload only jpeg, jpg and png files");
                    return View(team);
                }

                team.ImageUrl = team.ImageFile.SaveFile(_env.WebRootPath, "uploads/teams");

                string path = Path.Combine(_env.WebRootPath, "uploads/teams", existteam.ImageUrl);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

                existteam.ImageUrl = team.ImageUrl;
            }

            existteam.PositionId = team.PositionId;
            existteam.FbUrl = team.FbUrl;
            existteam.InstaUrl = team.InstaUrl;
            existteam.LnUrl = team.LnUrl;
            existteam.TwitterUrl = team.TwitterUrl;
            existteam.FullName = team.FullName;
            existteam.Description = team.Description;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Team existteam = _context.Teams.Include(x => x.Position).FirstOrDefault(x => x.Id == id);
            if (existteam == null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, "uploads/teams", existteam.ImageUrl);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

            _context.Teams.Remove(existteam);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
