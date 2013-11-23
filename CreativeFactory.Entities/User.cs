using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreativeFactory.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
