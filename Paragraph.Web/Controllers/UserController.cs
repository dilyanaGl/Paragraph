using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Paragraph.Data.Models;
using Paragraph.Services.DataServices;

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

        public async Task<IActionResult> SetAdmin()
        {
            var user = this.userService.SetRandomAdmin();
         
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await userManager.AddToRoleAsync(user, "Admin");

            return this.Content($"User with username {user.UserName} is set to Admin");
        }

        public async Task<IActionResult> SetUserRole()
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
            return this.Conflict($"Role user is added to the database");
        }

    }
}