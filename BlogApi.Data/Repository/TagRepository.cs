using BlogApi.Data.Models;
using BlogApi.Data.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Data.Repository
{
    public class TagRepository : ITagRepository
    {
        private BlogApiDbContext _context;

        public TagRepository(BlogApiDbContext context)
        {
            _context = context;
        }

        public List<Tag> GetAllTags()
        {
            return _context.Tags.ToList();
        }

        public Tag GetById(int id)
        {
            return _context.Tags.FirstOrDefault(t => t.Id == id);
        }

        public Tag GetByName(string name)
        {
            return _context.Tags.FirstOrDefault(t => t.Name == name);
        }

        public async Task AddTag(Tag tag)
        {
            var entry = _context.Entry(tag);
            if (entry.State == EntityState.Detached)
                await _context.Tags.AddAsync(tag);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateTag(Tag tag, UpdateTagQuery query)
        {
            if (!string.IsNullOrEmpty(query.NewName))
                tag.Name = query.NewName;

            var entry = _context.Entry(tag);
            if (entry.State == EntityState.Detached)
                _context.Tags.Update(tag);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveTag(int id)
        {
            _context.Tags.Remove(GetById(id));
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
