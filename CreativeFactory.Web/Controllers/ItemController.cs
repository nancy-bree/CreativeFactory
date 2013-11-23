using CreativeFactory.DAL;
using CreativeFactory.Entities;
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
        private IUnitOfWork unitOfWork;

        public ItemController(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
        }

        //
        // GET: /Item/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Item/Add

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
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(model);
        }

        //
        // GET: /Item/Details

        public ActionResult Details(int id = 1)
        {
            var item = unitOfWork.ItemRepository.GetByID(id);
            return View(item);
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
            var md = new MarkdownDeep.Markdown();
            md.ExtraMode = true;
            var html = md.Transform(model.Body);
            var item = new Item
            {
                Title = String.IsNullOrWhiteSpace(model.Title) ? "..." : model.Title,
                Body = html,
                ArticleId = model.ArticleId,
                Order = GetItemOrder(model.ArticleId)
            };
            unitOfWork.ItemRepository.Insert(item);
            unitOfWork.Save();
        }

        private int GetItemOrder(int articleId)
        {
            var article = unitOfWork.ArticleRepository.GetByID(articleId);
            if (article.Items.Count == 0) return 1;
            else return article.Items.OrderBy(x => x.Order).Last().Order + 1;
        }

        public JsonResult GetNewItemsOrder(string order)
        {
            var idList = order.Split(',').Select(x => Convert.ToInt32(x));
            int i = 1;
            foreach (var id in idList)
            {
                var item = unitOfWork.ItemRepository.GetByID(id);
                item.Order = i;
                unitOfWork.ItemRepository.Update(item);
                i++;
            }
            unitOfWork.Save();
            return Json(new { success = true });
        }
    }
}
