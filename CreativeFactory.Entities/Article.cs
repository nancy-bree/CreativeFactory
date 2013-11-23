using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreativeFactory.Entities
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Resources))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "TitleIsTooLong")]
        public string Title { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resources.Resources))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "DescriptionIsTooLong")]
        public string Description { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}
