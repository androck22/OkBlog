using BlogApi.Data.Models;
using BlogApi.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Data.Repository
{
    public interface ITagRepository
    {
        List<Tag> GetAllTags();
        public Tag GetById(int id);
        public Tag GetByName(string name);
        Task AddTag(Tag tag);
        Task UpdateTag(Tag tag, UpdateTagQuery query);
        Task RemoveTag(int id);
        Task<bool> SaveChangesAsync();
    }
}
