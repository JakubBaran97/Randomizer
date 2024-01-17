using System.ComponentModel.DataAnnotations;

namespace Randomizer.Models
{
    public class EditPasswordDto
    {
       

        [MaxLength(20)]
        [MinLength(8)]
        public string Password { get; set; }


        [MaxLength(20)]
        [MinLength(8)]
        public string NewPassword { get; set; }

        [MaxLength(20)]
        [MinLength(8)]
        public string ConfirmNewPassword { get; set; }

       
    }
}
