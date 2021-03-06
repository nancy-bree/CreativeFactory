﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CreativeFactory.Entities
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Resources))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "TitleIsTooLong")]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "BodyFieldCannotBeEmpty", ErrorMessageResourceType = typeof (Resources.Resources))]
        public string Body { get; set; }

        public DateTime CreatedDate { get; set; }

        public int Order { get; set; }

        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public virtual ICollection<Rating> Votes { get; set; }
    }
}
