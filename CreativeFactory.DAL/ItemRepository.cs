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
        public ItemRepository(CreativeFactoryContext context) : base(context)
        {
        }
    }
}
