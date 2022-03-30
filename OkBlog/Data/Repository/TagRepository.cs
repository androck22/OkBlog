using OkBlog.Models.Db;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkBlog.Data.Repository
{
    public class TagRepository : ITagRepository
    {
        private BlogDbContext _context;

        public TagRepository(BlogDbContext context)
        {
            _context = context;
        }

        public List<Tag> GetAllTags()
        {
            return _context.Tags.ToList();
        }

        public Tag GetTag(int id)
        {
            return _context.Tags.FirstOrDefault(t => t.Id == id);
        }

        //public List<Tag> GetPostTags(Post post)
        //{
        //    return _context.Tags.ToList().Where(t => t.Id == post.Id);
        //}

        public void AddTag(Tag tag)
        {
            _context.Tags.Add(tag);
        }

        public void UpdateTag(Tag tag)
        {
            _context.Tags.Update(tag);
        }

        public void RemoveTag(int id)
        {
            _context.Tags.Remove(GetTag(id));
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
