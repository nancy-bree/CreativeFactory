using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreativeFactory.Web.Models
{
    public class NewItemViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Resources))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "TitleIsTooLong")]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "BodyFieldCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resources))]
        public string Body { get; set; }

        public int ArticleId { get; set; }

        public string CookieToken { get; set; }
    }
}