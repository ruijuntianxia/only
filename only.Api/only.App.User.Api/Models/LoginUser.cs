using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace only.App.User.Api.Models
{
    public class LoginUser
    {
       
        [Display(Name ="用户名")]
        public string userName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string passWord { get; set; }

        [Display(Name = "记住")]
        public bool remember { get; set; }
    }
}
