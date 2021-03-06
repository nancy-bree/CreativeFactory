﻿using CreativeFactory.DAL;
using CreativeFactory.Entities;
using CreativeFactory.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        // GET: /Item/Add

        [Authorize]
        public ActionResult Add(int articleId)
        {
            ViewBag.ArticleId = articleId;
            if (Request.Cookies["_autosave"] != null)
            {
                var model = GetNewItemTextFromCookies();
                return View(model);
            }
            else
            {
                var model = SetNewItemTextToCookies();
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
                    DeleteCookies();
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
            if (Request.Cookies[model.Id.ToString()] != null)
            {
                model.Body = DraftService.GetDraft(Request.Cookies[model.Id.ToString()].Value);
            }
            else
            {
                var cookie = new HttpCookie(model.Id.ToString())
                {
                    Value = model.Id.ToString(),
                    Expires = DateTime.Now.AddDays(1)
                };
                Response.Cookies.Add(cookie);
            }
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
                    EditItem(model);
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
            DeleteCookiesAndSavedDrafts(id);
            ClearCache();
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult GetStats(/*string monthName*/)
        {
            //var month = DateTime.ParseExact(monthName, "MMMM", CultureInfo.InvariantCulture).Month;
            var time = DateTime.Now;
            var dict = HttpRuntime.Cache.Get("ActivityStats") as IDictionary<int, int>;
            if (dict == null)
            {
                dict =
                _unitOfWork.ItemRepository.GetItemsStatistics(/*month*/time.Month, time.Year);
                HttpRuntime.Cache["ActivityStats"] = dict;
            }
            var dates = dict.Keys.ToArray();
            var count = dict.Values.ToArray();
            return Json(new { count = count, dates = dates }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveDraft(FormCollection form)
        {
            var name = form[1];
            if (Request.Cookies[name] != null)
            {
                var filename = Request.Cookies[name].Value;
                var content = form[0];
                DraftService.SaveDraft(filename, content);
            }
            return Json(new { success = true });
        }

        #region privateMethods
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
            ClearCache();
        }

        private void EditItem(ItemViewModel model)
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
            if (Request.Cookies[model.Id.ToString()] != null)
            {
                var cookie = Request.Cookies[model.Id.ToString()];
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Set(cookie);
            }
            DraftService.DeleteDraft(model.Id.ToString());
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

        private static void ClearCache()
        {
            if (HttpRuntime.Cache["PopularArticlesAndVotes"] != null)
            {
                HttpRuntime.Cache.Remove("PopularArticlesAndVotes");
            }
            if (HttpRuntime.Cache["AllArticles"] != null)
            {
                HttpRuntime.Cache.Remove("AllArticles");
            }
            if (HttpRuntime.Cache["ActivityStats"] != null)
            {
                HttpRuntime.Cache.Remove("ActivityStats");
            }
        }

        private NewItemViewModel SetNewItemTextToCookies()
        {
            var cookieToken = Guid.NewGuid().ToString();
            var model = new NewItemViewModel { CookieToken = cookieToken };
            var cookie = new HttpCookie("_autosave")
            {
                Value = cookieToken,
                Expires = DateTime.Now.AddDays(1)
            };
            var titleCookie = new HttpCookie("_item-title")
            {
                Expires = DateTime.Now.AddDays(1)
            };
            Response.Cookies.Add(cookie);
            Response.Cookies.Add(titleCookie);
            return model;
        }

        private NewItemViewModel GetNewItemTextFromCookies()
        {
            var title = String.Empty;
            if (Request.Cookies["_item-title"] != null)
            {
                title = Request.Cookies["_item-title"].Value;
            }
            var cookie = Request.Cookies["_autosave"].Value;
            var model = new NewItemViewModel
            {
                CookieToken = cookie,
                Body = DraftService.GetDraft(cookie),
                Title = title
            };
            return model;
        }

        private void DeleteCookies()
        {
            if (Request.Cookies["_autosave"] != null)
            {
                var cookie = Request.Cookies["_autosave"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Set(cookie);
            }
            if (Request.Cookies["_item-title"] != null)
            {
                var cookie = Request.Cookies["_item-title"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Set(cookie);
            }
        }

        private void DeleteCookiesAndSavedDrafts(int id)
        {
            if (Request.Cookies[id.ToString()] != null)
            {
                var cookie = Request.Cookies[id.ToString()];
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Set(cookie);
            }
            DraftService.DeleteDraft(id.ToString());
        }
        #endregion
    }
}
