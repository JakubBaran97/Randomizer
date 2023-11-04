using System.ComponentModel.DataAnnotations;

namespace Randomizer.Models
{
    public class RegisterUserDto
    {

        [Required]
        [MaxLength(45)]
        public string Name { get; set; }

        [MaxLength(45)]
        public string Email { get; set; }

        [MaxLength(20)]
        [MinLength(8)]
        public string Password { get; set; }

        [MaxLength(20)]
        [MinLength(8)]
        public string ConfirmPassword { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(45)]
        public string? Nationality { get; set; }


    }
}
