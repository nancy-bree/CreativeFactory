using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreativeFactory.Entities;

namespace CreativeFactory.DAL
{
    public class RatingRepository : Repository<Rating>
    {
        public RatingRepository(CreativeFactoryContext context) : base(context) { }

        public int GetOneItemRating(int id)
        {
            //select count(*)
            //from Ratings
            //where ItemId = 13
            //group by ItemId
            var query = from r in _context.Rating
                        group r by r.ItemId
                        into g
                        where g.Key == id
                        let count = g.Count()
                        select count;
            return query.First();
        }

        public bool HasVoted(int userId, int itemId)
        {
            var query = _context.Rating.FirstOrDefault(x => x.UserId == userId && x.ItemId == itemId);
            return query != null;
        }
    }
}
