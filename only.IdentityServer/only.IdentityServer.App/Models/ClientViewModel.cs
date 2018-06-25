using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace only.IdentityServer.App.Models
{
    public class ClientViewModel
    {
        [Required]
        [Display(Name = "客户端ID")]
        public string ClientId { get; set; }
        [Required]
        [Display(Name = "客户端名称")]
        public string ClientName { get; set; }
        [Required]
        [Display(Name = "允许访问类型")]
        public string AllowedGrantTypes { get; set; }

        
        [Display(Name = "客户端加密")]
        public string ClientSecrets { get; set; }

        [Required]
        [Display(Name = "访问地址")]
        public string RedirectUris { get; set; }

        [Required]
        [Display(Name = "注销后访问地址")]
        public string PostLogoutRedirectUris { get; set; }

        [Required]
        [Display(Name = "允许权限")]
        public string AllowedScopes { get; set; }

        
        [Display(Name = "是否启用acess")]
        public bool AllowOfflineAccess { get; set; }

        
        [Display(Name = "初始路径")]
        public string AllowedCorsOrigins { get; set; }

        
        [Display(Name = "是否启用token")]
        public bool AllowAccessTokensViaBrowser { get; set; }
      
    }
}
