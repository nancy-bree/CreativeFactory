using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreativeFactory.Entities;

namespace CreativeFactory.DAL
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(CreativeFactoryContext context) : base(context)
        {
        }
    }
}
