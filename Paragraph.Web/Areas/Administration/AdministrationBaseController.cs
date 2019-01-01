using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Paragraph.Web.Areas.Administration
{
    [Area("Administration")]
    public class AdministrationBaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}