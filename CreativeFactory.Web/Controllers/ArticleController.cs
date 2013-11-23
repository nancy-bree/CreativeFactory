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
    [Authorize]
    public class ArticleController : BaseController
    {
        private IUnitOfWork unitOfWork;

        public ArticleController(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
        }

        //
        // GET: /Article/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Article/Add

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
                    return RedirectToAction("Index", "Home"/*, new { id = WebSecurity.CurrentUserId }*/);
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(model);
        }

        //
        // GET: /Article/Edit

        public ActionResult Edit(int id)
        {
            var article = unitOfWork.ArticleRepository.GetByID(id);
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
                    var user = unitOfWork.UserRepository.GetByID(WebSecurity.CurrentUserId);
                    var articleToUpdate = unitOfWork.ArticleRepository.GetByID(model.Id);
                    articleToUpdate.Description = model.Description;
                    articleToUpdate.User = user;
                    if ((model.Tags != null) && !String.IsNullOrWhiteSpace(model.Tags))
                    {
                        SetTags(model.Tags, articleToUpdate);
                    }
                    else
                    {
                        unitOfWork.ArticleRepository.DeleteArticleTag(articleToUpdate.Id);
                    }
                    unitOfWork.ArticleRepository.Update(articleToUpdate);
                    unitOfWork.Save();
                    return RedirectToAction("Details", "Article", new { id = model.Id });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to edit article. Try again, and if the problem persists, see your system administrator.");
            }
            return View(model);
        }

        //
        // GET: /Article/Details

        public ActionResult Details(int id = 1, int page = 1)
        {
            var article = unitOfWork.ArticleRepository.GetByID(id);
            ViewBag.ArticleId = article.Id;
            var model = new ArticleDetailsViewModel(article.Title, article.Description, article.CreatedDate, article.Tags, article.Items, page);
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
            unitOfWork.ArticleRepository.Insert(article);
            unitOfWork.Save();
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
                var tmp = unitOfWork.TagRepository.GetTagByName(tag.Trim());
                if (tmp == null)
                {
                    tmp = new Tag
                    {
                        Name = tag.Trim()
                    };
                    unitOfWork.TagRepository.Insert(tmp);
                    unitOfWork.Save();
                }
                article.Tags.Add(tmp);
            }
        }
    }
}
