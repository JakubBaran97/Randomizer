using System.ComponentModel.DataAnnotations;

namespace Randomizer.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }

        public int? MenuId { get; set; }

        public virtual Menu Menu { get; set; }

        public int? CreatedById { get; set; }


    }
}
