using BlogVerse.Web.Data;
using BlogVerse.Web.IRepository;
using BlogVerse.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BlogVerse.Web.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext dbContext;

        public TagRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await dbContext.Tags.AddAsync(tag);
            await dbContext.SaveChangesAsync();
            return tag;

        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = dbContext.Tags.FirstOrDefault(x => x.Id == id);
            if (existingTag == null)
            {
                return null;
            }

            dbContext.Tags.Remove(existingTag);
            await dbContext.SaveChangesAsync();
            return existingTag;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {            
            return await dbContext.Tags.ToListAsync(); ;
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await dbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Guid id, Tag tag)
        {
            var existingTag = await dbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
            
            if (existingTag == null)
            {
                return null;
            }

            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;
            await dbContext.SaveChangesAsync();

            return existingTag;

        }
    }
}
