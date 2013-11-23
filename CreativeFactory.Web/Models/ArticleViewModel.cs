using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreativeFactory.Web.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Resources))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "TitleIsTooLong")]
        public string Title { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resources.Resources))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "DescriptionIsTooLong")]
        public string Description { get; set; }

        public string Tags { get; set; }
    }
}