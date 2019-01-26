using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Paragraph.Services.DataServices
{
    using Data.Common;
    using Data.Models;
    using Models.Request;
    using Services.Mapping;

    public class RequestService : IRequestService
    {
        private readonly IRepository<Request> requestRepository;
        private readonly IRepository<ParagraphUser> userRepository;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ParagraphUser> userManager;

        public RequestService(IRepository<Request> requestRepository, IRepository<ParagraphUser> userRepository, RoleManager<IdentityRole> roleManager, UserManager<ParagraphUser> userManager)
        {
            this.requestRepository = requestRepository;
            this.userRepository = userRepository;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        //TO DO: Check why requests are not added to the database

        public void MakeRequest(string username, IdentityRole role)
        {

            var adminUser = this.userManager.GetUsersInRoleAsync("Admin").Result.FirstOrDefault();

            var user = this.userRepository.All().FirstOrDefault(p => p.UserName == username);

            var request = new Request
            {
                RequestReceiver = adminUser,
                RequestSender = user,
                Role = role
            };

            this.requestRepository.AddAsync(request);
            this.requestRepository.SaveChangesAsync();
        }

        public IEnumerable<string> GetAllRoles(string username)
        {
            var user = this.userRepository.All()
                .FirstOrDefault(p => p.UserName == username);
            var requestedRoles = this.requestRepository.All().Where(p => p.RequestSenderId == user.Id)
                .Select(p => p.Role.Name)
                .ToArray();
            var roles = this.userManager.GetRolesAsync(user).Result;
            return this.roleManager.Roles
                .Where(p => !roles.Contains(p.Name) && p.Name != "User" && p.Name != "Admin")
                .Where(p => !requestedRoles.Contains(p.Name))
                .Select(p => p.Name)
                .ToArray();
        }

        public IEnumerable<ListRequestModel> GetUserRequests(string username)
        {
            return this.requestRepository.All()
               .Where(p => p.RequestSender.UserName == username)
                .To<ListRequestModel>()
                .ToArray();
        }

        public IEnumerable<ListRequestModel> GetAdminRequests(string username)
        {
            return this.requestRepository.All()
                .Where(p => p.RequestReceiver.UserName == username && p.Status == Status.Pending)
                .To<ListRequestModel>()
                .ToArray();
        }

        public string GetRole(int requestId)
        {
            var request = this.requestRepository.All().SingleOrDefault(p => p.Id == requestId);
            var role = this.roleManager.FindByIdAsync(request.RoleId).Result;

            return role.Name;
        }

        public string GetUserId(int requestId)
        {
            var request = this.requestRepository.All().SingleOrDefault(p => p.Id == requestId);
            return request.RequestSenderId;
        }

        public async Task<bool> GrantRequest(string userId, string role)
        {
            var user = this.userRepository.All().SingleOrDefault(p => p.Id == userId);

            var result = await this.userManager
                     .AddToRoleAsync(user, role);

            return result.Succeeded;




        }

        public void SetRequestStatus(int requestId)
        {
            var request = this.requestRepository.All().SingleOrDefault(p => p.Id == requestId);
            request.Status = Status.Approved;
            this.requestRepository.SaveChangesAsync();
        }



    }
}