using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Uni_Movie.Areas.Identity.Data;
using Uni_Movie.Data;
using Uni_Movie.Models;
using Uni_Movie.ViewModels;

namespace Uni_Movie.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext db;
		private readonly UserManager<ApplicationUser> userManager;

		public HomeController(ApplicationDbContext _db,UserManager<ApplicationUser> _userManager)
		{
			db = _db;
			userManager = _userManager;
		}
		public async Task< IActionResult> Index(int pageNumber)
		{
			if (User.Identity.IsAuthenticated)
			{
				var user = await userManager.FindByNameAsync(User.Identity.Name);
				var recommendedCount= db.VisitedGenres.Where(x => x.userId == user.Id).Count();
				ViewData["recommendedCount"] = recommendedCount;
			}
			var count = db.Movies.ToList().Count();
			ViewData["count"] = count;
			HomeViewModel model = new HomeViewModel()
			{
				genreList = db.Genres.Select(x => new SelectListItem { Text = x.title, Value = x.Id.ToString() }),
				movieList = db.Movies.Include(x => x.genre).ToList(),
				movie = null
			};
			if (pageNumber > 0)
			{
				model.paginationDTO.PageNumber = pageNumber;
			}
			model.movieList = model.movieList.Skip(model.paginationDTO.PageSize * (model.paginationDTO.PageNumber - 1)).Take(model.paginationDTO.PageSize);
			return View(model);


		}

		public async Task<IActionResult> SearchMovie(HomeViewModel viewModel)
		{
			if (String.IsNullOrEmpty(viewModel.movie.Title))
			{
				ModelState.AddModelError("Title", "Search title can not be empty");
				viewModel.genreList = db.Genres.Select(x => new SelectListItem { Text = x.title, Value = x.Id.ToString() });
				viewModel.movieList = db.Movies.Include(x => x.genre).ToList();
				viewModel.movie = null;
				return View("Index", viewModel);
			}
			if (viewModel.movie != null && !String.IsNullOrEmpty(viewModel.movie.Title))
			{
				var movies = await db.Movies.Where(x => x.Title.ToLower().Contains(viewModel.movie.Title.ToLower())).Include(x => x.genre).ToListAsync();
				HomeViewModel model = new HomeViewModel() { movieList = movies };
				if (model.movieList.Count() == 0)
				{
					TempData["notFound"] = "!Nothing found";
					return View("Index", model);
				}
				else
				{

					return View("Index", model);
				}
			}
			return RedirectToAction("Index", viewModel);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Filter(HomeViewModel viewModel)
		{
			if (viewModel.movie != null && viewModel.movie.genreId != 0)
			{


				var movies = await db.Movies.Where(x => x.genreId == viewModel.movie.genreId).Include(x => x.genre).ToListAsync();
				if (movies.Count() > 0)
				{
					HomeViewModel model = new HomeViewModel()
					{
						movieList = movies
					};
					return View("Index", model);
				}
				else
				{
					return NotFound();
				}
			}
			else
			{
				ModelState.AddModelError(String.Empty, "Please choose a genre");
				viewModel.genreList = db.Genres.Select(x => new SelectListItem { Text = x.title, Value = x.Id.ToString() });
				viewModel.movieList = db.Movies.Include(x => x.genre).ToList();
				viewModel.movie = null;
				return View("Index", viewModel);
			}
		}

	}
}