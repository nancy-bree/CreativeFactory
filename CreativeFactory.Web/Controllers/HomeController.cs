using CreativeFactory.DAL;
using CreativeFactory.Entities;
using CreativeFactory.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreativeFactory.Web.Services;
using PagedList;
using CreativeFactory.Web.Properties;
using CreativeFactory.Web.Models;

namespace CreativeFactory.Web.Controllers
{
    [HandleError]
    public class HomeController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //
        // GET: /CreativeFactory/

        public ActionResult Index(int page = 1)
        {
            var model = GetIndexPage(page);
            return View(model);
        }

        //[OutputCache(Duration = 600, VaryByParam = "userId")]
        public ActionResult MyArticles(int userId, int page = 1)
        {
            var list = GetMyArtclesList(userId);
            return View(list
                .ToPagedList(page, Settings.Default.ArticlesPerPage));
        }

        public ActionResult AllArticles(int page = 1)
        {
            var newList = GetAllArticlesList();
            return View(newList
                .ToPagedList(page, Settings.Default.ArticlesPerPage));
        }


        public ActionResult SetCulture(string returnUrl, string culture)
        {
            var oldCulture = returnUrl.Substring(1, 2);
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            RouteData.Values["culture"] = culture;  // set culture
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture") {Value = culture, Expires = DateTime.Now.AddYears(1)};
            }
            Response.Cookies.Add(cookie);
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl.Replace(oldCulture, culture));
            }
            return RedirectToAction("Index", "Home");
        }

        #region privateMethods
        private IEnumerable<ArticleUnitViewModel> GetMyArtclesList(int userId)
        {
            var userArticles =
                    _unitOfWork.ArticleRepository.Get().Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedDate);
            ViewBag.UserId = userId;
            var list = ArticleService.ArticleUnitViewModelList(_unitOfWork, userArticles);
            return list;
        }

        private IEnumerable<ArticleUnitViewModel> GetAllArticlesList()
        {
            var articles = HttpRuntime.Cache.Get("AllArticles") as IEnumerable<Article>;
            if (articles == null)
            {
                articles =
                    _unitOfWork.ArticleRepository.Get(x => x.OrderByDescending(y => y.CreatedDate));
                HttpRuntime.Cache["AllArticles"] = articles;
            }
            var newList = ArticleService.ArticleUnitViewModelList(_unitOfWork, articles);
            return newList;
        }

        private MainPageViewModel GetIndexPage(int page)
        {
            var articles = HttpRuntime.Cache.Get("AllArticles") as IEnumerable<Article>;
            if (articles == null)
            {
                articles =
                    _unitOfWork.ArticleRepository.Get(x => x.OrderByDescending(y => y.CreatedDate));
                HttpRuntime.Cache["AllArticles"] = articles;
            }
            var popularArticles = ArticleService.ArticleUnitViewModelList(_unitOfWork, articles).Where(x => x.Votes > 0).OrderByDescending(x => x.Votes);
            var tags = HttpRuntime.Cache.Get("AllTags") as IEnumerable<Tag>;
            if (tags == null)
            {
                tags =
                    _unitOfWork.TagRepository.Get(x => x.OrderBy(y => y.Name));
                HttpRuntime.Cache["AllTags"] = tags;
                //HttpRuntime.Cache.Add("AllTags", tags, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration, CacheItemPriority.Normal, new CacheItemRemovedCallback())
            }
            ViewBag.TotalArticlesCount = _unitOfWork.ArticleRepository.Get().Count();
            var model = new MainPageViewModel(tags, popularArticles, page);
            return model;
        }
        #endregion
    }
}
