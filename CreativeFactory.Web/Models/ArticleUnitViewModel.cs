using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CreativeFactory.Entities;

namespace CreativeFactory.Web.Models
{
    public class ArticleUnitViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public int Items { get; set; }

        public int Votes { get; set; }

        public ICollection<Tag> Tags { get; set; }
    }
}