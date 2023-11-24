using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using Uni_Movie.Data;
using Uni_Movie.Models;
using Uni_Movie.ViewModels;

namespace Uni_Movie.Controllers
{
	public class MovieController : Controller
	{
		private readonly ApplicationDbContext db;

		public MovieController(ApplicationDbContext _db)
		{
			db = _db;
		}
		public IActionResult Index()
		{
			IEnumerable<Movie> movies=db.Movies.Include(x=>x.genre).ToList();
			return View(movies);
		}
		
	}
}
