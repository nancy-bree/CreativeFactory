using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreativeFactory.Entities;

namespace CreativeFactory.DAL
{
    public class ItemRepository : Repository<Item>
    {
        public ItemRepository(CreativeFactoryContext context) : base(context) { }

        public IDictionary<int, int> GetItemsStatistics(int month, int year)
        {
            var query = (from a in _context.Item
                        where a.CreatedDate.Month == month && a.CreatedDate.Year == year
                        group a by a.CreatedDate.Day into g
                        select new {Day=g.Key, Items= g.Count()}).ToDictionary(x => x.Day, x => x.Items);
            return query;
        }

    }
}
