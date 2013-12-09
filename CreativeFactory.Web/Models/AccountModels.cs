using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace CreativeFactory.Web.Models
{
    public class LocalPasswordModel
    {
        [Required(ErrorMessageResourceName = "CurrentPasswordFieldCannotBeEmpty", ErrorMessageResourceType = typeof (Resources.Resources))]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword", ResourceType = typeof (Resources.Resources))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "NewPasswordFieldCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resources))]
        [StringLength(100, ErrorMessageResourceName = "PasswordIsTooShort", ErrorMessageResourceType = typeof(Resources.Resources), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof (Resources.Resources))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof (Resources.Resources))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordDontMatch", ErrorMessageResourceType = typeof(Resources.Resources))]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessageResourceName = "UsernameFieldCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Username", ResourceType = typeof(Resources.Resources))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "UsernameIsTooLong")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "PasswordFieldCannotBeEmpty", ErrorMessageResourceType = typeof (Resources.Resources))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof (Resources.Resources))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof (Resources.Resources))]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessageResourceName = "UsernameFieldCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Username", ResourceType = typeof(Resources.Resources))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "UsernameIsTooLong")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessage = null)]
        [Required(ErrorMessageResourceName = "EmailFieldCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resources))]
        [StringLength(254, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EmailIsTooLong")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "PasswordFieldCannotBeEmpty", ErrorMessageResourceType = typeof (Resources.Resources))]
        [StringLength(100, ErrorMessageResourceName = "PasswordIsTooShort", ErrorMessageResourceType = typeof (Resources.Resources), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof (Resources.Resources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof (Resources.Resources))]
        [Compare("Password", ErrorMessageResourceName = "PasswordDontMatch", ErrorMessageResourceType = typeof (Resources.Resources))]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordConfirmModel
    {
        public string Token { get; set; }

        [Required(ErrorMessageResourceName = "NewPasswordFieldCannotBeEmpty", ErrorMessageResourceType = typeof(Resources.Resources))]
        [StringLength(100, ErrorMessageResourceName = "PasswordIsTooShort", ErrorMessageResourceType = typeof(Resources.Resources), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(Resources.Resources))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Resources))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordDontMatch", ErrorMessageResourceType = typeof(Resources.Resources))]
        public string ConfirmPassword { get; set; }
    }
}
