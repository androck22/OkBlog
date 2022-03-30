using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OkBlog.Models.Db;
using OkBlog.Models.Db.Comments;
using System;

namespace OkBlog.Data
{
    public class BlogDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
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
