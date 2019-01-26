using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Paragraph.Services.DataServices.Models.Request;
using System.Threading.Tasks;

namespace Paragraph.Services.DataServices
{
    public interface IRequestService
    {
        IEnumerable<ListRequestModel> GetAdminRequests(string username);
        IEnumerable<string> GetAllRoles(string username);
        string GetRole(int requestId);
        IEnumerable<ListRequestModel> GetUserRequests(string username);
        Task<bool> GrantRequest(string username, string role);
        void MakeRequest(string username, IdentityRole role);
        void SetRequestStatus(int requestId);
        string GetUserId(int requestId);
    }
}