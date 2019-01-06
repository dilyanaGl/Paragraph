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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Paragraph.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ParagraphUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRequestService requestService;
        private readonly IUserService userService;

        public UserController(UserManager<ParagraphUser> userManager, RoleManager<IdentityRole> roleManager, IUserService userService, IRequestService requestService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userService = userService;
            this.requestService = requestService;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [Authorize]
        public IActionResult Profile()
        {
            var model = this.userService.Details(this.User.Identity.Name);
            
            this.ViewData["Roles"] = this.requestService.GetAllRoles(this.User.Identity.Name)
                .Select(p => new SelectListItem
                {
                    Text = p, 
                    Value = p
                });               

            return this.View(model);
        }     
    }
}