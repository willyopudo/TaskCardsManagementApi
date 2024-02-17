using System.ComponentModel.DataAnnotations;

namespace LogiceaCardsManagementApp2.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string email { get; set; }

        /*[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Minimum eight characters, at least one uppercase letter,"
             + "one lowercase letter, one number and one special character")]*/
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
