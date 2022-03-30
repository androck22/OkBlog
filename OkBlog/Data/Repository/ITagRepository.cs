using OkBlog.Models.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OkBlog.Data.Repository
{
    public interface ITagRepository
    {
        List<Tag> GetAllTags();
        public Tag GetTag(int id);
        void AddTag(Tag tag);
        void UpdateTag(Tag tag);
        void RemoveTag(int id);
        Task<bool> SaveChangesAsync();
    }
}
