using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OkBlog.Models.Db.Comments;

namespace OkBlog.Data.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private BlogDbContext _context;

        public CommentRepository(BlogDbContext context)
        {
            _context = context;
        }

        public List<MainComment> GetAllComments()
        {
            return _context.MainComments.ToList();
        }

        public MainComment GetComment(int id)
        {
            return _context.MainComments.FirstOrDefault(c => c.Id == id);
        }

        public void AddComment(MainComment comment)
        {
            _context.MainComments.Add(comment);
        }

        public void UpdateComment(MainComment comment)
        {
            _context.MainComments.Update(comment);
        }

        public void RemoveComment(int id)
        {
            _context.MainComments.Remove(GetComment(id));
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
