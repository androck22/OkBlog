using System.Collections.Generic;
using System.Threading.Tasks;
using OkBlog.Models.Db.Comments;

namespace OkBlog.Data.Repository
{
    public interface ICommentRepository
    {
        List<MainComment> GetAllComments();
        public MainComment GetComment(int id);
        void AddComment(MainComment tag);
        void UpdateComment(MainComment tag);
        void RemoveComment(int id);
        Task<bool> SaveChangesAsync();
    }
}
