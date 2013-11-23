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

namespace CreativeFactory.Web.Controllers
{
    [HandleError]
    public class HomeController : BaseController
    {
        private IUnitOfWork unitOfWork;

        public HomeController(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyArticles(int userId, int page = 1)
        {
            ViewBag.UserId = userId;
            return View(unitOfWork.ArticleRepository
                .GetAllUserArticles(userId)
                .ToPagedList(page, Settings.Default.ArticlesPerPage));
        }

        public ActionResult AllArticles(int page = 1)
        {
            return View(unitOfWork.ArticleRepository
                .Get(orderBy: x => x.OrderByDescending(y => y.CreatedDate))
                .ToPagedList(page, Settings.Default.ArticlesPerPage));
        }

        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            RouteData.Values["culture"] = culture;  // set culture
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }
    }
}
