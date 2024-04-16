using BlogVerse.Web.Models.Domain;

namespace BlogVerse.Web.IRepository
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag?> GetAsync(Guid id);
        Task<Tag> AddAsync(Tag tag);
        Task<Tag?> UpdateAsync(Guid id, Tag tag);
        Task<Tag?> DeleteAsync(Guid id);
    }
}
