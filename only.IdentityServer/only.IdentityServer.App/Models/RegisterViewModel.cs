using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace only.IdentityServer.App.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name ="姓")]
        public string FamilyName { get; set; }

        [Display(Name = "名")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "邮箱")]
        [DataType(DataType.EmailAddress)]
        public string UserMail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [Display(Name = "确认密码")]
        [DataType(DataType.Password)]
        [Compare("PassWord", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }



     

       
    }
}
