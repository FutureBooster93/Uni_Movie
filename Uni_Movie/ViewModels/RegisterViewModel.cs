using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Uni_Movie.ViewModels
{
	public class RegisterViewModel
	{
        [Required]
        public string firstname { get; set; }
        [Required]
		public string lastname { get; set; }
        [Required]
        [Remote("CheckEmail", "Account", ErrorMessage = "This email already exists")]
        public string EmailAddress { get; set; }
        [Required]
        public string password { get; set; }
    }
}
