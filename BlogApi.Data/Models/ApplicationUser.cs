using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BlogApi.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set;} = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime Created { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();


        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
