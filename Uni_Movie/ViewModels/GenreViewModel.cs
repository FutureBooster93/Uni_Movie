using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Uni_Movie.Models;

namespace Uni_Movie.ViewModels
{
	public class GenreViewModel
	{
        [ValidateNever]
        public IEnumerable<Genre> genres { get; set; }
        public Genre genre { get; set; }
    }
}
