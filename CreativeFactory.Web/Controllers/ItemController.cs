using System.Globalization;
using System.IO;
using System.Web.Caching;
using CreativeFactory.DAL;
using CreativeFactory.Entities;
using CreativeFactory.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreativeFactory.Web.Properties;
using CreativeFactory.Web.Services;
using WebMatrix.WebData;

namespace CreativeFactory.Web.Controllers
{
    public class ItemController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //
        // GET: /Item/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Item/Add

        [Authorize]
        public ActionResult Add(int articleId)
        {
            ViewBag.ArticleId = articleId;
            if (Request.Cookies["_autosave"] != null)
            {
                var cookie = Request.Cookies["_autosave"].Value;
                var model = new NewItemViewModel
                {
                    CookieToken = cookie,
                    Body = DraftService.GetDraft(cookie)
                };
                return View(model);
            }
            else
            {
                var cookieToken = Guid.NewGuid().ToString();
                var model = new NewItemViewModel { CookieToken = cookieToken };
                var cookie = new HttpCookie("_autosave")
                {
                    Value = cookieToken,
                    Expires = DateTime.Now.AddDays(1)
                };
                Response.Cookies.Add(cookie);
                return View(model);
            }
        }

        //
        // POST: /Item/Add

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(NewItemViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var item = new Item
                    {
                        Id = model.Id,
                        ArticleId = model.ArticleId,
                        Body = model.Body,
                        Title = model.Title
                    };
                    if (Request.Cookies["_autosave"] != null)
                    {
                        var cookie = Request.Cookies["_autosave"];
                        cookie.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Set(cookie);
                    }
                    DraftService.DeleteDraft(model.CookieToken);
                    AddItem(item);
                    return RedirectToAction("Details", "Article", new { id = model.ArticleId });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", Resources.Resources.UnableToSaveChanges);
            }
            return View(model);
        }

        //
        // GET: /Article/Edit

        [Authorize]
        public ActionResult Edit(int id)
        {
            var item = _unitOfWork.ItemRepository.GetByID(id);
            if (item.Article.UserId != WebSecurity.CurrentUserId) return RedirectToAction("Http403", "Error");
            var model = new ItemViewModel
            {
                Id = item.Id,
                Title = item.Title,
                Body = item.Body,
                ArticleId = item.ArticleId,
            };
            return View(model);
        }

        //
       //POST: /Article/Edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var article = _unitOfWork.ArticleRepository.GetByID(model.ArticleId);
                    var itemToUpdate = _unitOfWork.ItemRepository.GetByID(model.Id);
                    itemToUpdate.Article = article;
                    itemToUpdate.Title = model.Title;
                    var md = new MarkdownDeep.Markdown { ExtraMode = true };
                    var html = md.Transform(model.Body);
                    itemToUpdate.Body = html;
                    _unitOfWork.ItemRepository.Update(itemToUpdate);
                    _unitOfWork.Save();
                    return RedirectToAction("Details", "Article", new { id = model.ArticleId });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", Resources.Resources.UnableToSaveChanges);
            }
            return View(model);
        }

        //
        // GET: /Item/Details

        public ActionResult Details(int id = 1)
        {
            var item = _unitOfWork.ItemRepository.GetByID(id);
            return View(item);
        }

        //
        // POST: /Item/Delete

        [Authorize]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            _unitOfWork.ItemRepository.Delete(id);
            _unitOfWork.Save();
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult GetStats(/*string monthName*/)
        {
            //var month = DateTime.ParseExact(monthName, "MMMM", CultureInfo.InvariantCulture).Month;
            var time = DateTime.Now;
            var dict = _unitOfWork.ItemRepository.GetItemsStatistics(/*month*/time.Month, time.Year);
            var dates = dict.Keys.ToArray();
            var count = dict.Values.ToArray();
            return Json(new { count = count, dates = dates }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveDraft(FormCollection form)
        {
            // TODO: Save the form values and return a JSON result 
            // to indicate if the save went succesfully
            if (Request.Cookies["_autosave"] != null)
            {
                var filename = Request.Cookies["_autosave"].Value;
                var content = form[0];
                DraftService.SaveDraft(filename, content);
            }
            return Json(new { success = true });
        }

        private void AddItem(Item model)
        {
            var md = new MarkdownDeep.Markdown {ExtraMode = true};
            var html = md.Transform(model.Body);
            var item = new Item
            {
                Title = String.IsNullOrWhiteSpace(model.Title) ? "..." : model.Title,
                Body = html,
                ArticleId = model.ArticleId,
                Order = GetItemOrder(model.ArticleId)
            };
            _unitOfWork.ItemRepository.Insert(item);
            _unitOfWork.Save();
        }

        private int GetItemOrder(int articleId)
        {
            var article = _unitOfWork.ArticleRepository.GetByID(articleId);
            if (article.Items.Count == 0) return 1;
            return article.Items.OrderBy(x => x.Order).Last().Order + 1;
        }

        public JsonResult GetNewItemsOrder(string order)
        {
            var idList = order.Split(',').Select(x => Convert.ToInt32(x));
            int i = 1;
            foreach (var id in idList)
            {
                var item = _unitOfWork.ItemRepository.GetByID(id);
                item.Order = i;
                _unitOfWork.ItemRepository.Update(item);
                i++;
            }
            _unitOfWork.Save();
            return Json(new { success = true });
        }
    }
}
