using CreativeFactory.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using PagedList;
using CreativeFactory.Web.Properties;
using Postal;

namespace CreativeFactory.Web.Controllers
{
    [HandleError]
    [Authorize(Roles = "Administrator")]
    public class AdminController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //
        // GET: /Admin/Users

        public ActionResult Users(int page = 1)
        {
            return View(_unitOfWork.UserRepository.Get(x => x.OrderBy(y => y.UserName)).ToPagedList(page, Settings.Default.ArticlesPerPage));
        }

        //
        // POST: /Admin/Delete

        [HttpPost]
        public JsonResult Delete(string username)
        {
            var membership = (SimpleMembershipProvider)Membership.Provider;
            Roles.RemoveUserFromRoles(username, Roles.GetRolesForUser(username));
            bool wasDeleted = membership.DeleteAccount(username);
            wasDeleted = membership.DeleteUser(username, true);
            return Json(new { success = true });
        }

        //
        // POST: /Admin/ResetUserPassword

        [HttpPost]
        public JsonResult ResetUserPassword(string username, string emailAddress)
        {
            var confirmationToken = WebSecurity.GeneratePasswordResetToken(username);
            dynamic email = new Email("ChngPasswordEmail");
            email.To = emailAddress;
            email.UserName = username;
            email.ConfirmationToken = confirmationToken;
            email.Send();
            return Json(new { success = true });
        }

        //
        // POST: /Admin/ChangeUserRole

        [HttpPost]
        public JsonResult ChangeUserRole(string username, string act)
        {
            switch (act)
            {
                case "add":
                    Roles.AddUserToRole(username, "Administrator");
                    break;
                case "exclude":
                    Roles.RemoveUserFromRole(username, "Administrator");
                    break;
            }
            return Json(new { success = true });
        }

    }
}
