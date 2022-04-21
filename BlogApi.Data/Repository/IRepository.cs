using BlogApi.Data.Models;
using BlogApi.Data.Models.Comments;
using BlogApi.Data.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApi.Data.Repository
{
    public interface IRepository
    {
        List<Post> GetAllPosts();
        Task<Post> GetPostByTitle(string title);
        Post GetPostById(int id);
        Task AddPost(Post post);
        Task UpdatePost(Post post, ApplicationUser user, UpdatePostQuery query);
        Task UpdatePost(Post post);
        Task RemovePost(int id);
        Task AddSubComment(SubComment comment);
        Task<bool> SaveChangesAsync();
    }
}
