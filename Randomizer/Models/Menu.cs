using System.ComponentModel.DataAnnotations;

namespace Randomizer.Models
{
    public class Menu
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        
        public virtual List<Product> Products { get; set; }
        public int? CreatedById { get; set; }


    }
}
