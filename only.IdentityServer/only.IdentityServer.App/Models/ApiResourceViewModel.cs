using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace only.IdentityServer.App.Models
{
    public class ApiResourceViewModel
    {
        [Required]
        [Display(Name ="服务端ID")]
       public string Name { get; set; }
        [Required]
        [Display(Name = "服务端说明")]
        public string DisplayName { get; set; }
        [Required]
        [Display(Name = "服务端授权")]
        public string ClaimTypes { get; set; }
    }
}
