using CreativeFactory.DAL;
using CreativeFactory.Entities;
using CreativeFactory.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            var lol = _unitOfWork.ArticleRepository.GetPopularArticlesId();
            var tags = _unitOfWork.TagRepository.Get(x => x.OrderBy(y => y.Name));
            ViewBag.TotalArticlesCount = _unitOfWork.ArticleRepository.Get().Count();
            var popularArticles = _unitOfWork.ArticleRepository.Get();//RatingService.GetPopularPhotosList();
            var model = new MainPageViewModel(tags, popularArticles, page);
            return View(model);
        }

        public ActionResult MyArticles(int userId, int page = 1)
        {
            ViewBag.UserId = userId;
            return View(_unitOfWork.ArticleRepository
                .GetAllUserArticles(userId)
                .ToPagedList(page, Settings.Default.ArticlesPerPage));
        }

        public ActionResult AllArticles(int page = 1)
        {
            return View(_unitOfWork.ArticleRepository
                .Get(x => x.OrderByDescending(y => y.CreatedDate))
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
    }
}
