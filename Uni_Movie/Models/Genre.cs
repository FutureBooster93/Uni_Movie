using System.ComponentModel.DataAnnotations;

namespace Uni_Movie.Models
{
	public class Genre
	{
		public int Id { get; set; }
		[Required]
		public string title { get; set; }
        public List<Movie> Movies { get; set; }
        public List<VisitedGenre> visitedGenres { get; set; }
    }
}
