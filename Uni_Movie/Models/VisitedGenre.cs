using System.ComponentModel.DataAnnotations.Schema;
using Uni_Movie.Areas.Identity.Data;

namespace Uni_Movie.Models
{
	public class VisitedGenre
	{
		public int Id { get; set; }
        public string userId { get; set; }
		[ForeignKey("userId")]
        public ApplicationUser applicationUser { get; set; }
        public int genreId { get; set; }
		[ForeignKey("genreId")]
		public Genre genre { get; set; }
		public DateTime visitDatetime { get; set; }= DateTime.Now;
	}
}
