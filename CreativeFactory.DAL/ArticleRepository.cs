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
        /// Gets all user articles.
        /// </summary>
        /// <param name="userId">Uticle Id.</param>
        /// <returns>List of articles contains to a user.</returns>
        public IEnumerable<Article> GetAllUserArticles(int userId)
        {
            var query = Context.Article.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedDate);
            return query;
        }

        /// <summary>
        /// Gets all article tags.
        /// </summary>
        /// <param name="articleId">Article Id.</param>
        /// <returns>List of tags.</returns>
        public IEnumerable<Tag> GetArticleTags(int articleId)
        {
            var query = Context.Tag.Where(x => x.Articles.Any(y => y.Id == articleId));
            return query;
        }

        /// <summary>
        /// Removes all article tags.
        /// </summary>
        /// <param name="articleId">Article ID.</param>
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

        public IDictionary<int, int> GetPopularArticlesIdAndVotes()
        {
            //select Items.ArticleId, count(*) Votes
            //from Items
            //inner join Ratings
            //on items.Id = Ratings.ItemId
            //group by Items.ArticleId
            //order by Votes desc

            //var query = _context.Database
            //            .SqlQuery<int>("SELECT Items.ArticleId FROM Items " +
            //                           "INNER JOIN Ratings " +
            //                           "ON Items.Id = Ratings.ItemId " +
            //                           "GROUP BY Items.ArticleId " +
            //                           "ORDER BY COUNT(*) DESC");

            var query = (from item in Context.Item
                join rating in Context.Rating on item.Id equals rating.ItemId
                group item by item.ArticleId
                into result
                let votes = result.Count()
                orderby votes descending
                select new {Id = result.Key, Votes = votes}).ToDictionary(x => x.Id, x => x.Votes);

            return query;
        }
    }
}
