using System.ComponentModel.DataAnnotations;

namespace CreativeFactory.Web.Models
{
    public class ItemViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Resources))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "TitleIsTooLong")]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "BodyFieldCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resources))]
        public string Body { get; set; }

        public string Draft { get; set; }

        public int ArticleId { get; set; }
    }
}