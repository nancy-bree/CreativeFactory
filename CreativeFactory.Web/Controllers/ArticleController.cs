using CreativeFactory.DAL;
using CreativeFactory.Entities;
using CreativeFactory.Web.Models;
using CreativeFactory.Web.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace CreativeFactory.Web.Controllers
{
    [HandleError]
    public class ArticleController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //
        // GET: /Article/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Article/Add

        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        //
        // POST: /Article/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ArticleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AddArticle(model);
                    return RedirectToAction("MyArticles", "Home", new { userId = WebSecurity.CurrentUserId });
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
            var article = _unitOfWork.ArticleRepository.GetByID(id);
            if (article.UserId != WebSecurity.CurrentUserId) return RedirectToAction("Http403", "Error");
            var model = new ArticleViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Description = article.Description,
            };
            var tags = article.Tags.Select(item => item.Name).ToList();
            model.Tags = String.Join(", ", tags);
            return View(model);
        }

        //
        // POST: /Article/Edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _unitOfWork.UserRepository.GetByID(WebSecurity.CurrentUserId);
                    var articleToUpdate = _unitOfWork.ArticleRepository.GetByID(model.Id);
                    articleToUpdate.Title = model.Title;
                    articleToUpdate.Description = model.Description;
                    articleToUpdate.User = user;
                    if ((model.Tags != null) && !String.IsNullOrWhiteSpace(model.Tags))
                    {
                        SetTags(model.Tags, articleToUpdate);
                    }
                    else
                    {
                        _unitOfWork.ArticleRepository.DeleteArticleTag(articleToUpdate.Id);
                    }
                    _unitOfWork.ArticleRepository.Update(articleToUpdate);
                    _unitOfWork.Save();
                    return RedirectToAction("MyArticles", "Home", new { userId = WebSecurity.CurrentUserId });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", Resources.Resources.UnableToSaveChanges);
            }
            return View(model);
        }

        //
        // POST: /Article/Delete

        [Authorize]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            _unitOfWork.ArticleRepository.Delete(id);
            _unitOfWork.Save();
            return Json(new { success = true });
        }

        //
        // GET: /Article/Details

        public ActionResult Details(int id = 1)
        {
            var article = _unitOfWork.ArticleRepository.GetByID(id);
            ViewBag.ArticleId = article.Id;
            ViewBag.UserId = article.UserId;
            var model = new ArticleDetailsViewModel(article.Title, article.Description, article.CreatedDate, article.Tags, article.Items);
            return View(model);
        }

        private void AddArticle(ArticleViewModel model)
        {
            var article = new Article
            {
                Title = String.IsNullOrWhiteSpace(model.Title) ? "..." : model.Title,
                Description = model.Description,
                UserId = WebSecurity.CurrentUserId,
            };
            if (model.Tags != null)
            {
                SetTags(model.Tags, article);
            }
            _unitOfWork.ArticleRepository.Insert(article);
            _unitOfWork.Save();
        }

        private void SetTags(string p, Article article)
        {
            IEnumerable<string> tags = TagService.SplitTags(p).Distinct();
            if (article.Tags != null)
            {
                article.Tags.Clear();
            }
            else
            {
                article.Tags = new List<Tag>();
            }
            foreach (var tag in tags)
            {
                var tmp = _unitOfWork.TagRepository.GetTagByName(tag.Trim());
                if (tmp == null)
                {
                    tmp = new Tag
                    {
                        Name = tag.Trim()
                    };
                    _unitOfWork.TagRepository.Insert(tmp);
                    _unitOfWork.Save();
                }
                article.Tags.Add(tmp);
            }
        }
    }
}
