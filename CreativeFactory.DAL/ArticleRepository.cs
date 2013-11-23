using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreativeFactory.Entities;

namespace CreativeFactory.DAL
{
    public class ArticleRepository : Repository<Article>
    {
        public ArticleRepository(CreativeFactoryContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all user articls.
        /// </summary>
        /// <param name="userId">Uticle Id.</param>
        /// <returns>List of articles contains to a user.</returns>
        public IEnumerable<Article> GetAllUserArticles(int userId)
        {
            var query = _context.Article.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedDate);
            return query;
        }

        /// <summary>
        /// Gets all article tags.
        /// </summary>
        /// <param name="photoId">Article Id.</param>
        /// <returns>List of tags.</returns>
        public IEnumerable<Tag> GetArticleTags(int articleId)
        {
            var query = _context.Tag.Where(x => x.Articles.Any(y => y.Id == articleId));
            return query;
        }

        /// <summary>
        /// Removes all article tags.
        /// </summary>
        /// <param name="photoId">Article ID.</param>
        public void DeleteArticleTag(int articleId)
        {
            var photo = this.GetByID(articleId);
            if (photo.Tags == null) return;
            var query = GetArticleTags(articleId);
            foreach (var item in query)
            {
                photo.Tags.Remove(item);
            }
        }
    }
}
