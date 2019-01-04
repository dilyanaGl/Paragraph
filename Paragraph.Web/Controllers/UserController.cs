using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Paragraph.Data.Models;
using Paragraph.Services.DataServices;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Paragraph.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ParagraphUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserService userService;

        public UserController(UserManager<ParagraphUser> userManager, RoleManager<IdentityRole> roleManager, IUserService userService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Profile()
        {
            var model = this.userService.Details(this.User.Identity.Name);
            var role = userManager.GetRolesAsync(this.userManager.GetUserAsync(this.User).Result);

            if (role == null)
            {
                model.Role = "User";
            }
            else
            {
                model.Role = String.Join(", ", role.Result.ToArray());
            }

            return this.View(model);
        }

        [HttpPost]
        public IActionResult RequestRights()
        {
            this.ViewData["Message"] = "Your request has been sent to the admin!";
            return this.Index();
        }



    }
}