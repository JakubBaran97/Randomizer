using Microsoft.AspNetCore.Identity;
using Randomizer.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Randomizer.Entities
{
    public class User
    {
        public int Id { get; set; }



        public string? Name { get; set; }

       

        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }
       
     
        public string? PasswordHash { get; set; }

        public string? Nationality { get; set; }

        

        public virtual List<Menu> Menu { get; set; }
    }
}
