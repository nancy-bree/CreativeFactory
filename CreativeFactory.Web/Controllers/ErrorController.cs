using System.Web.Mvc;

namespace CreativeFactory.Web.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/Http404

        public ActionResult Http404(string aspxerrorpath)
        {
            ViewData["error_path"] = aspxerrorpath;
            return View();
        }

        //
        // GET: /Error/Http403

        public ActionResult Http403()
        {
            return View();
        }
    }
}
