using BlogApi.Data.Models;
using BlogApi.Data.Models.Comments;
using BlogApi.Data.Queries;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi.Data.Repository
{
    public class Repository : IRepository
    {
        private BlogApiDbContext _context;

        public Repository(BlogApiDbContext context)
        {
            _context = context;
        }

        public List<Post> GetAllPosts()
        {
            var result = _context.Posts.ToList();

            return result;
        }

        public Task<Post> GetPostByTitle(string title)
        {
            return _context.Posts.FirstOrDefaultAsync(p => p.Title == title);
        }

        public Post GetPostById(int id)
        {
            return _context.Posts
                //.Include(t => t.Tags)
                //.Include(mc => mc.MainComments)
                //.ThenInclude(sc => sc.SubComments)
                .FirstOrDefault(p => p.Id == id);
        }

        public async Task AddPost(Post post)
        {
            //await _context.Posts.AddAsync(post);
            var entry = _context.Entry(post);
            if (entry.State == EntityState.Detached)
                await _context.Posts.AddAsync(post);

            await _context.SaveChangesAsync();
        }

        public async Task UpdatePost(Post post, ApplicationUser user, UpdatePostQuery query)
        {
            post.UserId = user.Id;
            post.User = user;

            if (!string.IsNullOrEmpty(query.NewTitle))
                post.Title = query.NewTitle;
            if (!string.IsNullOrEmpty(query.NewBody))
                post.Body = query.NewBody;

            var entry = _context.Entry(post);
            if (entry.State == EntityState.Detached)
                _context.Posts.Update(post);

            await _context.SaveChangesAsync();
        }

        public async Task UpdatePost(Post post)
        {
            var entry = _context.Entry(post);
            if (entry.State == EntityState.Detached)
                _context.Posts.Update(post);

            await _context.SaveChangesAsync();
        }

        public async Task RemovePost(int id)
        {
            _context.Posts.Remove(GetPostById(id));
            await _context.SaveChangesAsync();
        }

        public async Task AddSubComment(SubComment comment)
        {
            var entry = _context.Entry(comment);
            if (entry.State == EntityState.Detached)
                await _context.SubComments.AddAsync(comment);

            await _context.SaveChangesAsync();
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
