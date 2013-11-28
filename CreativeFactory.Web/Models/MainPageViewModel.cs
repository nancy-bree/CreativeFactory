using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CreativeFactory.Entities;
using CreativeFactory.Web.Properties;
using PagedList;

namespace CreativeFactory.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class MainPageViewModel
    {
        public IEnumerable<Tag> TagCloud { get; set; }

        public IPagedList<Article> PopularArticles { get; set; }

        public MainPageViewModel(IEnumerable<Tag> tagCloud, IEnumerable<Article> popularArticles, int page = 1)
        {
            TagCloud = tagCloud;
            PopularArticles = popularArticles.ToPagedList(page, Settings.Default.ArticlesPerPage);
        }
    }
}