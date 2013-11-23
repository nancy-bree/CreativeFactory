using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreativeFactory.Entities
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "TagNameFieldCannotBeEmpty", ErrorMessageResourceType = typeof (Resources.Resources))]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "TagNameIsTooLong")]
        public string Name { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
