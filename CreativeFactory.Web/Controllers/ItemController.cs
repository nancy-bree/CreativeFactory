using CreativeFactory.DAL;
using CreativeFactory.Entities;
using CreativeFactory.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            return View();
        }

        //
        // POST: /Item/Add

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Item model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AddItem(model);
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
                    return RedirectToAction("Details", "Item", new { id = model.Id });
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
        public JsonResult GetStats()
        {
            var time = DateTime.Now;
            var dict = _unitOfWork.ItemRepository.GetItemsStatistics(time.Month, time.Year);
            var dates = dict.Keys.ToArray();
            var count = dict.Values.ToArray();
            return Json(new { count = count, dates = dates }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveDraft(FormCollection form)
        {
            // TODO: Save the form values and return a JSON result 
            // to indicate if the save went succesfully
            var content = form[0];
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
