using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Uni_Movie.Data;
using Uni_Movie.Models;

namespace Uni_Movie.Controllers
{
	public class HomeController : Controller
	{
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            IEnumerable<Movie> movies = db.Movies.Include(x => x.genre).ToList();
            return View(movies);
        }
    }
}