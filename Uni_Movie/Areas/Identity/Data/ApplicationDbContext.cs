using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uni_Movie.Areas.Identity.Data;
using Uni_Movie.Models;

namespace Uni_Movie.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}
	public DbSet<ApplicationUser> ApplicationUsers { get; set; }
	public DbSet<Movie> Movies { get; set; }
	public DbSet<Genre> Genres { get; set; }
	public DbSet<VisitedGenre> VisitedGenres { get; set; }
	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.Entity<Genre>().HasData(
			new Genre { Id = 1, title = "Comedy" },
			new Genre { Id = 2, title = "Horror" },
			new Genre { Id = 3, title = "Sci-Fi" },
			new Genre { Id = 4, title = "Documentary" },
			new Genre { Id = 5, title = "Drama" }

		);


		// Customize the ASP.NET Identity model and override the defaults if needed.
		// For example, you can rename the ASP.NET Identity table names and more.
		// Add your customizations after calling base.OnModelCreating(builder);
	}
}
