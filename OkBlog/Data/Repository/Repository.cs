using Microsoft.EntityFrameworkCore;
using OkBlog.Models.Db;
using OkBlog.Models.Db.Comments;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkBlog.Data.Repository
{
    public class Repository : IRepository
    {
        private BlogDbContext _context;

        public Repository(BlogDbContext context)
        {
            _context = context;
        }

        public List<Post> GetAllPosts()
        {
            return _context.Posts.ToList();
        }

        public List<Post> GetAllPosts(string category)
        {
            return _context.Posts
                .Where(post => post.Category.ToLower().Equals(category.ToLower()))
                .ToList();
        }

        public Post GetPost(int id)
        {
            return _context.Posts
                .Include(t => t.Tags)
                .Include(mc => mc.MainComments)
                .ThenInclude(sc => sc.SubComments)
                .FirstOrDefault(p => p.Id == id);
        }

        public void AddPost(Post post)
        {
            _context.Posts.Add(post);
        }

        public void UpdatePost(Post post)
        {
            _context.Posts.Update(post);
        }

        public void RemovePost(int id)
        {
            _context.Posts.Remove(GetPost(id));
        }

        public void AddSubComment(SubComment comment)
        {
            _context.SubComments.Add(comment);
        }

        public async Task<bool> SaveChangesAsync()
        {
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        
    }
}
