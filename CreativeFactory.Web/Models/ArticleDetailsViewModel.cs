using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using CreativeFactory.Entities;
using CreativeFactory.Web.Properties;

namespace CreativeFactory.Web.Models
{
    public class ArticleDetailsViewModel
    {
        public string Title { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public IPagedList<Item> Items { get; set; }

        public ArticleDetailsViewModel(string title, string description, DateTime date, ICollection<Tag> tags, ICollection<Item> items, int page = 1)
        {
            Title = title;
            Tags = tags;
            Description = description;
            CreatedDate = date;
            Items = items.OrderBy(x => x.Order).ToPagedList(page, Settings.Default.ItemsPerPage);
        }
    }
}