using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Imaging;
using System.Drawing;
using Uni_Movie.Models;
using Uni_Movie.ViewModels;
using Uni_Movie.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Humanizer.Bytes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Uni_Movie.Areas.Identity.Data;

namespace Uni_Movie.Controllers
{
	[Authorize(Roles = "admins")]
	public class MovieController : Controller
	{
		private readonly ApplicationDbContext db;
		private readonly UserManager<ApplicationUser> userManager;

		public MovieController(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
		{
			db = _db;
			userManager = _userManager;
		}
		public IActionResult Index()
		{
			IEnumerable<Movie> movies;
			movies = db.Movies.Include(x => x.genre).ToList();
			return View(movies);
		}
		public IActionResult Create()
		{
			MovieCreateViewModel model = new MovieCreateViewModel()
			{
				movie = new MovieViewModel(),
				genreList = db.Genres.Select(x => new SelectListItem
				{
					Text = x.title,
					Value = x.Id.ToString()
				})
			};

			return View(model);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(MovieCreateViewModel model)
		{
			if (ModelState.IsValid)
			{
				Movie movie = new()
				{
					Title = model.movie.Title,
					Year = model.movie.Year,
					Director = model.movie.Director,
					Description = model.movie.Description,
					Rate = model.movie.Rate,
					genreId = model.movie.genreId
				};
				if (model.movie.MovieImage != null)
				{
					string extention = Path.GetExtension(model.movie.MovieImage.FileName).ToLower();
					if (extention == ".jpg" || extention == ".jpeg" || extention == ".png")
					{
						if (model.movie.MovieImage.Length <= 5 * Math.Pow(1024, 2))
						{
							byte[] buffer = new byte[model.movie.MovieImage.Length];
							model.movie.MovieImage.OpenReadStream().Read(buffer, 0, buffer.Length);
							movie.MovieImage = buffer;
							//MemoryStream memoryStream = new MemoryStream(buffer);
							//Image image = Image.FromStream(memoryStream);
							//Bitmap bmp = new Bitmap(image, 120, 100);
							//MemoryStream thumbnail = new MemoryStream();
							//bmp.Save(thumbnail, ImageFormat.Jpeg);
							//byte[] thumbnailBytes = thumbnail.ToArray();
							//movie.MovieImage = thumbnailBytes;
						}
					}
				}
				db.Movies.Add(movie);
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			else { return View(model); }
		}
		public async Task<IActionResult> Update(int id)
		{
			Movie movie = await db.Movies.FindAsync(id);
			if (movie != null)
			{
				MovieUpdateViewModel model = new MovieUpdateViewModel()
				{
					movie = new MovieViewModel()
					{
						Title = movie.Title,
						Description = movie.Description,
						Director = movie.Director,
						Year = movie.Year,
						Rate = movie.Rate,
						Id = id,
						Image = movie.MovieImage,
						genreId = movie.genreId
					},
					genreList = await db.Genres.ToListAsync()
				};
				return View(model);
			}
			else { return NotFound(); }
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(MovieUpdateViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (model.movie.MovieImage == null)
				{
					Movie movie = new()
					{
						Id = model.movie.Id,
						Title = model.movie.Title,
						Description = model.movie.Description,
						Director = model.movie.Director,
						Year = model.movie.Year,
						Rate = model.movie.Rate,
						genreId = model.movie.genreId,
						MovieImage = model.movie.Image,
					};
					db.Movies.Update(movie);
					await db.SaveChangesAsync();
					return RedirectToAction("Index");
				}
				else
				{

					Movie movie = new()
					{
						Id = model.movie.Id,
						Title = model.movie.Title,
						Description = model.movie.Description,
						Director = model.movie.Director,
						Year = model.movie.Year,
						Rate = model.movie.Rate,
						genreId = model.movie.genreId
					};
					string extention = Path.GetExtension(model.movie.MovieImage.FileName).ToLower();
					if (extention == ".jpg" || extention == ".jpeg" || extention == ".png")
					{
						if (model.movie.MovieImage.Length <= 5 * Math.Pow(1024, 2))
						{
							byte[] buffer = new byte[model.movie.MovieImage.Length];
							model.movie.MovieImage.OpenReadStream().Read(buffer, 0, buffer.Length);
							movie.MovieImage = buffer;
						}
					}
					db.Movies.Update(movie);
					await db.SaveChangesAsync();
					return RedirectToAction("Index");
				}
			}
			else
			{

				return View(model);
			}
		}
		public IActionResult Delete(int id)
		{
			if (id == 0) { return NotFound(); }
			var movie = db.Movies.Find(id);
			if (movie != null)
			{
				db.Movies.Remove(movie);
				db.SaveChanges();
				return Json(new { success = true });
			}
			else
			{
				return Json(new { success = false });
			}
		}
		[AllowAnonymous]
		public async Task<IActionResult> Details(int? id)
		{
			var movie = await db.Movies.Where(x => x.Id == id).Include(x => x.genre).FirstOrDefaultAsync();
			if (movie != null)
			{
				if (User.Identity.IsAuthenticated)
				{
					var user = await userManager.FindByNameAsync(User.Identity.Name);
					if (user != null)
					{
						var visitedGenres = db.VisitedGenres.Where(x => x.userId == user.Id).Count();
						if (visitedGenres <= 20)
						{
							VisitedGenre visitedGenre = new()
							{
								userId = user.Id,
								genreId = movie.genreId
							};
							db.VisitedGenres.Add(visitedGenre);
							db.SaveChanges();
						}
						else
						{
							var lastEntery = db.VisitedGenres.Where(x => x.userId == user.Id).OrderBy(x => x.visitDatetime).First();
							db.Remove(lastEntery);
							db.SaveChanges();
							VisitedGenre visitedGenre = new()
							{
								userId = user.Id,
								genreId = movie.genreId
							};
							db.VisitedGenres.Add(visitedGenre);
							db.SaveChanges();
						}
					}
				}
			}
			return View(movie);
		}
	}
}
