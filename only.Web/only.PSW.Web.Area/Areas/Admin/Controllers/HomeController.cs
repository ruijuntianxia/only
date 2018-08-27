using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using only.PSW.Web.Area.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace only.PSW.Web.Area.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController:Controller
    {
        [CustomerAuthorize]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
