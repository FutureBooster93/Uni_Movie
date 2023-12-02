using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Uni_Movie.Areas.Identity.Data;
using Uni_Movie.Middleware;
using Uni_Movie.ViewModels;

namespace Uni_Movie.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly RoleManager<IdentityRole> roleManager;

		public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, RoleManager<IdentityRole> _roleManager)
		{
			userManager = _userManager;
			signInManager = _signInManager;
			roleManager = _roleManager;
		}
		public async Task<IActionResult> Register()
		{
			await DbSeeder.IdentityInitializer(userManager, roleManager);
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				ApplicationUser user = await userManager.FindByNameAsync(model.EmailAddress);
				if (user == null)
				{
					user = new ApplicationUser()
					{
						firstName = model.firstname,
						lastName = model.lastname,
						Email = model.EmailAddress,
						UserName = model.EmailAddress,

					};
					var result = await userManager.CreateAsync(user, model.password);
					var roleStatus = await userManager.AddToRoleAsync(user, "customers");
					if (result.Succeeded && roleStatus.Succeeded)
					{
						TempData["success"] = ".You have been registered successfully";
						return RedirectToAction("Index", "Home");
					}
					else
					{
						TempData["error"] = ".There was an error in registeration process";

					}
				}
				else
				{
					TempData["error"] = ".This user already exists";

				}
			}
			else
			{
				return View(model);
			}
			return View();
		}
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByNameAsync(model.EmailAddress);
				if (user != null)
				{
					var result = await signInManager.PasswordSignInAsync(user, model.password, false, false);
					if (result.Succeeded)
					{
						TempData["success"] = "Successfully signed in";
						return RedirectToAction("Index", "Home");
					}
					else
					{

						ModelState.AddModelError("EmailAddress", "Username or password is invalid");
						return View(model);
					}
				}
				else
				{
					ModelState.AddModelError("EmailAddress", "Username or password is invalid");
					return View(model);
				}
			}
			else { return View(model); }
		}
		public async Task<IActionResult> LogOut()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
		public async Task<IActionResult> CheckEmail(string emailAddress)
		{
			ApplicationUser user = await userManager.FindByNameAsync(emailAddress);
			if (user == null)
				return Json(true);
			else return Json(false);
		}
	}
}
