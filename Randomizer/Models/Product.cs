namespace Randomizer.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? MenuId { get; set; }

        public virtual Menu Menu { get; set; }

        public int? CreatedById { get; set; }


    }
}
