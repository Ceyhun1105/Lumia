using Lumia.Areas.Admin.ViewModels;
using Lumia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lumia.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class AdminManager : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminManager(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel adminVM)
        {
            if (!ModelState.IsValid) return View(adminVM);

            AppUser user = await _userManager.FindByNameAsync(adminVM.UserName);

            if (user is null)
            {
                ModelState.AddModelError("", "UserName or Password is invalid");
                return View(adminVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, adminVM.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password is invalid");
                return View(adminVM);
            }

            return RedirectToAction("index", "dashboard");
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {

                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("index", "home", new { area = "" });
        }








        /*        public async Task<IActionResult> CreateAdmin()
                {
                    AppUser admin = new AppUser
                    {
                        UserName = "Admin2023",
                        FullName = "Admin"
                    };

                    var result = await _userManager.CreateAsync(admin, "Admin2023");

                    return Ok(result);
                }
                public async Task<IActionResult> createrole()
                {
                    IdentityRole role1 = new IdentityRole("Admin");
                    IdentityRole role2 = new IdentityRole("Member");


                    await _roleManager.CreateAsync(role1);
                    await _roleManager.CreateAsync(role2);

                    return Ok("Created roles");
                }

                public async Task<IActionResult> setrole()
                {
                    var user = await _userManager.FindByNameAsync("Admin2023");
                    var result = await _userManager.AddToRoleAsync(user,"Admin");

                    return Ok(result);
                }*/
    }
}
