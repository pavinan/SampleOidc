
using System.ComponentModel.DataAnnotations;

namespace SampleOidc.Identity.API.Models.Account
{
    public class LoginViewModel
    {
        [Required, EmailAddress]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe {get;set;}

        public string ReturnUrl { get; set; }
    }
}