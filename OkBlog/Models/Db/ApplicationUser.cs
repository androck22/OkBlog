using Microsoft.AspNetCore.Identity;
using System;

namespace OkBlog.Models.Db
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set;} = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime Created { get; set; }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
