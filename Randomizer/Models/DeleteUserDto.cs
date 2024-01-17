using System.ComponentModel.DataAnnotations;

namespace Randomizer.Models
{
    public class DeleteUserDto
    {
        [MaxLength(20)]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
