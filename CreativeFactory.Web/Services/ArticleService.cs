using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CreativeFactory.DAL;
using CreativeFactory.Entities;
using CreativeFactory.Web.Models;

namespace CreativeFactory.Web.Services
{
    public static class ArticleService
    {
        public static IEnumerable<ArticleUnitViewModel> ArticleUnitViewModelList(IUnitOfWork uow, IEnumerable<Article> articles)
        {
            var votesList = HttpRuntime.Cache.Get("PopularArticlesAndVotes") as IDictionary<int, int>;
            if (votesList == null)
            {
                votesList = uow.ArticleRepository.GetPopularArticlesIdAndVotes();
                HttpRuntime.Cache["PopularArticlesAndVotes"] = votesList;
            }
            var newList = new List<ArticleUnitViewModel>();
            foreach (var item in articles)
            {
                newList.Add(new ArticleUnitViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    UserId = item.UserId,
                    Username = item.User.UserName,
                    Items = item.Items.Count,
                    Tags = item.Tags,
                    Votes = votesList.Where(x => x.Key == item.Id).Select(x => x.Value).FirstOrDefault()
                });
            }
            return newList;
        }
    }
}