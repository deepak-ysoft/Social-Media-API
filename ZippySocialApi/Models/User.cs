using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ZippySocialApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression("^[A-Za-z\\s]+(?: [A-Za-z0-9\\s]+)*$", ErrorMessage = "Please enter a valid name!!")]
        public string Name { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*#?&]{8,}$", ErrorMessage = "Must have upper and lowercash, specail char., number and at least 8 character.")]
        public string Password { get; set; }
        [NotMapped]
        [Compare("Password")]
        public string ConPassword { get; set; }
        [Required]
        //[RegularExpression(@"^\\+?[0-9 ]{10,12}$\r\n", ErrorMessage ="Phone number not valid.")]
        public string Phone { get; set; }
        [Required]
        [AgeRange(18, 100, ErrorMessage = "Minimum age 18 and maximum 100.")]
        public DateTime DOB { get; set; }
        [Required]
        public string Gender { get; set; }
        public string AboutYou { get; set; }
        public string? ImagePath { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
public class AgeRangeAttribute : ValidationAttribute
{
    private readonly int _minAge;
    private readonly int _maxAge;

    public AgeRangeAttribute(int minAge, int maxAge)
    {
        _minAge = minAge;
        _maxAge = maxAge;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null)
        {
            DateTime dob = (DateTime)value;
            var today = DateTime.Today;
            var age = today.Year - dob.Year;

            // Adjust for cases where the birthday hasn't occurred yet this year
            if (dob > today.AddYears(-age)) age--;

            // Check if the age is within the allowed range
            if (age >= _minAge && age <= _maxAge)
            {
                return ValidationResult.Success;
            }

            // Return error if the age is not within the valid range
            return new ValidationResult($"Age must be between {_minAge} and {_maxAge} years old.");
        }

        return new ValidationResult("Invalid date value.");
    }

}

