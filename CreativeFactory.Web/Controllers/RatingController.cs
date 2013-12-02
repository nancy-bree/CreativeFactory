using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreativeFactory.DAL;
using CreativeFactory.Entities;

namespace CreativeFactory.Web.Controllers
{
    public class RatingController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public RatingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public JsonResult HasVoted(int userId, int itemId)
        {
            var result = _unitOfWork.RatingRepository.HasVoted(userId, itemId);
            return Json(new {result = result});
        }

        [HttpPost]
        public JsonResult GetItemRating(int itemId)
        {
            var result = _unitOfWork.ItemRepository.GetByID(itemId).Votes.Count;
            return Json(new {result = result});
        }

        [HttpPost]
        public JsonResult AddRemoveVote(int userId, int itemId, string action)
        {
            switch (action)
            {
                case "add":
                    _unitOfWork.RatingRepository.Insert(new Rating {UserId = userId, ItemId = itemId});
                    break;
                case "remove":
                    _unitOfWork.RatingRepository.Delete(_unitOfWork.RatingRepository.Get().FirstOrDefault(x => x.UserId == userId && x.ItemId == itemId));
                    break;
                default:
                    return Json(new {success = false});
            }
            _unitOfWork.Save();
            return Json(new {success = true});
        }

    }
}
