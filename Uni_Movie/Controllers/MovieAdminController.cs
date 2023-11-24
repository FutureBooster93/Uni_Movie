using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Imaging;
using System.Drawing;
using Uni_Movie.Models;
using Uni_Movie.ViewModels;
using Uni_Movie.Data;

namespace Uni_Movie.Controllers
{
	public class MovieAdminController : Controller
	{
		private readonly ApplicationDbContext db;

		public MovieAdminController(ApplicationDbContext _db)
        {
			db = _db;
		}
		public IActionResult Index() { return View(); }
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
							MemoryStream memoryStream = new MemoryStream(buffer);
							Image image = Image.FromStream(memoryStream);
							Bitmap bmp = new Bitmap(image, 200, 300);
							MemoryStream thumbnail = new MemoryStream();
							bmp.Save(thumbnail, ImageFormat.Jpeg);
							byte[] thumbnailBytes = thumbnail.ToArray();
							movie.MovieImage = thumbnailBytes;
						}
					}
				}
				db.Movies.Add(movie);
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			else { return View(model); }
		}
	}
}
