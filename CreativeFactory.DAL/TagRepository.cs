using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreativeFactory.Entities;

namespace CreativeFactory.DAL
{
    /// <summary>
    /// Defines data access operations for Tag entity.
    /// </summary>
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(CreativeFactoryContext context) : base(context) { }

        /// <summary>
        /// Returns tag by its name.
        /// </summary>
        /// <param name="name">Tag name.</param>
        /// <returns>Tag.</returns>
        public Tag GetTagByName(string name)
        {
            return _context.Tag.FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Get list of photos tagged <typeparamref name="tagId"/>.
        /// </summary>
        /// <param name="tagId">Tag ID.</param>
        /// <returns>List of photos.</returns>
        public IEnumerable<Article> GetArticlesByTag(int tagId)
        {
            var query = _context.Article.Where(x => x.Tags.Any(y => y.Id == tagId));
            return query;
        }
    }
}
