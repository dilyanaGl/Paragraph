using Paragraph.Data.Models;
using System.Threading.Tasks;

namespace Paragraph.Services.DataServices
{
    using Models.User;

    public interface IUserService
    {
        Task<ParagraphUser> SetRandomAdmin();
        UserDetailsModel Details(string username);
    }
}