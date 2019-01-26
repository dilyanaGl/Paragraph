using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Paragraph.Web.Controllers
{
    using Services.DataServices;
    using Data.Models;
    using Services.DataServices.Models.Request;

    public class RequestController : Controller
    {

        private readonly UserManager<ParagraphUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserService userService;
        private readonly IRequestService requestService;

        public RequestController(UserManager<ParagraphUser> userManager, RoleManager<IdentityRole> roleManager, IUserService userService, IRequestService requestService)
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

        //[Authorize(Roles ="Admin")]
        //public async Task<IActionResult> SetModeratorRole()
        //{
        //    await roleManager.CreateAsync(new IdentityRole("Writer"));
        //    return this.Content($"Created role Writer");
        //}

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult MakeRequest(RequestModel model)
        {
            if(!roleManager.RoleExistsAsync(model.Role).Result)
            {
                this.ViewData["Error"] = "Please choose a valid role!";
                return this.RedirectToAction("Profile", "User");
            };
            var role = this.roleManager.FindByNameAsync(model.Role).Result;
            this.requestService.MakeRequest(this.User.Identity.Name, role);

            return this.RedirectToAction("Profile", "User");
        }

        [Authorize(Roles="Admin")]
       // [ValidateAntiForgeryToken]
        public IActionResult GrantRequest(int id)
        {
            var roleName = this.requestService.GetRole(id);
            var requestSenderId = this.requestService.GetUserId(id);

           bool success = this.requestService.GrantRequest(requestSenderId, roleName).Result;
            if(success)
            {
                this.requestService.SetRequestStatus(id);
            }

            return this.RedirectToAction("Profile", "User");
        }

       
    }
}