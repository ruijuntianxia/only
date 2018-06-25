﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace only.PSW.Web.Area.Common
{
    public class CustomerAuthorizeAttribute : AuthorizeAttribute
    {
        public const string CustomerAuthenticationScheme = "CustomerAuthenticationScheme";
        public CustomerAuthorizeAttribute()
        {
            this.AuthenticationSchemes = CustomerAuthenticationScheme;
        }
    }
}
