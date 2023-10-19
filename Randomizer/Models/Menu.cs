namespace Randomizer.Models
{
    public class Menu
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<Product> Products { get; set; }

        

        public int? CreatedById { get; set; }


    }
}
