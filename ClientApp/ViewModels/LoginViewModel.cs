using System.ComponentModel.DataAnnotations;

namespace ClientApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }

        public bool LoginFailureHidden { get; set; } = true;

    }
}
