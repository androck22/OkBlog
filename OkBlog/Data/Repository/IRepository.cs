using OkBlog.Models.Db;
using OkBlog.Models.Db.Comments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OkBlog.Data.Repository
{
    public interface IRepository
    {
        List<Post> GetAllPosts();
        Post GetPost(int id);
        void AddPost(Post post);
        void UpdatePost(Post post);
        void RemovePost(int id);
        void AddSubComment(SubComment comment);
        Task<bool> SaveChangesAsync();
    }
}
