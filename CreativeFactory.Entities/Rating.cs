using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreativeFactory.Entities
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ItemId { get; set; }

        //public int Vote { get; set; }

        public User User { get; set; }

        public Item Item { get; set; }
    }
}
