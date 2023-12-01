using System.ComponentModel.DataAnnotations;

namespace Uni_Movie.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage ="Please enter Username/Email address")]
        public string EmailAddress { get; set; }
		[Required(ErrorMessage = "Please enter password")]
		public string password { get; set; }
    }
}
