using BlogApi.Data.Models;
using BlogApi.Data.Models.Comments;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Data
{
    public class BlogApiDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public BlogApiDbContext(DbContextOptions<BlogApiDbContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<MainComment> MainComments { get; set; }

        public DbSet<SubComment> SubComments { get; set; }
    }
}
