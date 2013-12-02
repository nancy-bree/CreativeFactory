using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreativeFactory.DAL;
using CreativeFactory.Web.Properties;
using PagedList;

namespace CreativeFactory.Web.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //
        // GET: /Search/

        public ActionResult Index(string searchString, int page = 1)
        {
            //if (!string.IsNullOrEmpty(searchString))
            //{
                ViewBag.Title = searchString;
                return
                    View(_unitOfWork.ItemRepository.FindInItems(searchString)
                        .ToPagedList(page, Settings.Default.ItemsPerPage));
            //}
        }

    }
}
