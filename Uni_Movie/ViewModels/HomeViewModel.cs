using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Uni_Movie.Models;

namespace Uni_Movie.ViewModels
{
	public class HomeViewModel
	{
        [ValidateNever]
        public IEnumerable<SelectListItem> genreList { get; set; }
        [ValidateNever]
        public IEnumerable<Movie> movieList { get; set; }
        public Movie movie { get; set; }
    }
}
