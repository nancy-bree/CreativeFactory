using PagedList;
using CreativeFactory.DAL;
using System.Linq;
using System.Web.Mvc;

namespace CreativeFactory.Web.Controllers
{
    [HandleError]
    public class TagController : BaseController
    {
        private IUnitOfWork unitOfWork;

        public TagController(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
        }

        //
        // GET: /Tag/

        public ActionResult Index(int id = 1, int page = 1)
        {
            ViewBag.Tag = unitOfWork.TagRepository.GetByID(id);
            return View(unitOfWork.ArticleRepository.Get().Where(x => x.Tags.Any(y => y.Id == id)).ToPagedList(page, 30));
        }

        //
        // GET: /Tag/GetTags

        public JsonResult GetTags(string term)
        {
            var tags = unitOfWork.TagRepository.Get().Where(x => x.Name.ToUpper().Contains(term.ToUpper())).Select(x => x.Name).ToArray();
            return Json(tags, JsonRequestBehavior.AllowGet);
        }

    }
}
