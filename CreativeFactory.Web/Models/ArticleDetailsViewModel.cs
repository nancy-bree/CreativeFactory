using System;
using System.Collections.Generic;
using System.Linq;
using CreativeFactory.Entities;

namespace CreativeFactory.Web.Models
{
    public class ArticleDetailsViewModel
    {
        public string Title { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public IEnumerable<Item> Items { get; set; }

        public ArticleDetailsViewModel(string title, string description, DateTime date, ICollection<Tag> tags, IEnumerable<Item> items)
        {
            Title = title;
            Tags = tags;
            Description = description;
            CreatedDate = date;
            Items = items.OrderBy(x => x.Order);
        }
    }
}