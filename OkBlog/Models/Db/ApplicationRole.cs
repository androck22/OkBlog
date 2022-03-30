using Microsoft.AspNetCore.Identity;
using System;

namespace OkBlog.Models.Db
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; } = null;

        public ApplicationRole() { }
        public ApplicationRole(string name, string description) 
        {
            Name = name; 
            Description = description;            
        }
    }
}
