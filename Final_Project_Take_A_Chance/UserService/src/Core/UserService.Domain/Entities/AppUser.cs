using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public required string Name { get; set; }
        public string? Surname { get; set; }
        public decimal Account { get; set; }
        public DateTime BirthDate { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        //relational

        public ICollection<AccountTopUp> AccountTopUps { get; set; }
    }
}
