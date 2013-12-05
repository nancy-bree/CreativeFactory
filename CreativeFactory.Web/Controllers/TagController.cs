using CreativeFactory.Web.Services;
using PagedList;
using CreativeFactory.DAL;
using System.Linq;
using System.Web.Mvc;

namespace CreativeFactory.Web.Controllers
{
    [HandleError]
    public class TagController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //
        // GET: /Tag/

        public ActionResult Index(int id = 1, int page = 1)
        {
            ViewBag.Tag = _unitOfWork.TagRepository.GetByID(id);
            var articles = _unitOfWork.ArticleRepository.Get().Where(x => x.Tags.Any(y => y.Id == id));
            var list = ArticleService.ArticleUnitViewModelList(_unitOfWork, articles);
            return View(list.ToPagedList(page, 30));
        }

        //
        // GET: /Tag/GetTags
        public JsonResult GetTags(string term)
        {
            var tags = _unitOfWork.TagRepository.Get().Where(x => x.Name.ToUpper().Contains(term.ToUpper())).Select(x => x.Name).ToArray();
            return Json(tags, JsonRequestBehavior.AllowGet);
        }

    }
}
