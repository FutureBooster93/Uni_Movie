using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uni_Movie.Models
{
	public class Movie
	{
		public int Id { get; set; }
		[Required]
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime Year { get; set; }
		[Required]
        public byte[] MovieImage { get; set; }
        [Required]
		public double Rate { get; set; }
		[Required]
		public string Director { get; set; }
		public int genreId { get; set; }
		[ForeignKey("genreId")]
		public Genre genre { get; set; }
	}
}
