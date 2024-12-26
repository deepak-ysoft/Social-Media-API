using System.ComponentModel.DataAnnotations;

namespace ZippySocialApi.Models
{
    public class LoginVM
    {
        public int? id { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email address.")]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*#?&]{8,}$", ErrorMessage = "Invalid Password.")]
        public string Password { get; set; }
    }
}
