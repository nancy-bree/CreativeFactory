using CreativeFactory.DAL;
using CreativeFactory.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult Add(/*int articleId*/)
        {
            //ViewBag.ArticleId = articleId;
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
                    var md = new MarkdownDeep.Markdown();
                    md.ExtraMode = true;
                    var html = md.Transform(model.Body);
                    AddItem(model);
                    return RedirectToAction("Index", "Home"/*, new { id = WebSecurity.CurrentUserId }*/);
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(model);
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
            /*var item = new Item
            {
                Title = model.Title,
                UserId = WebSecurity.CurrentUserId
            };
            if (model.Tags != null)
            {
                SetTags(model.Tags, article);
            }
            unitOfWork.ArticleRepository.Insert(article);
            unitOfWork.Save();*/
        }

    }
}
