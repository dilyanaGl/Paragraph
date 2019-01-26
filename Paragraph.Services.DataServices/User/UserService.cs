using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Paragraph.Services.DataServices
{
    using Data.Common;
    using Data.Models;
    using Models.User;
    using Models.Article;
    using Services.Mapping;

    public class UserService : IUserService
    {
        private readonly IRepository<ParagraphUser> userRepository;
        private readonly IRepository<Article> articleRepository;
        private readonly UserManager<ParagraphUser> userManager;


        public UserService(IRepository<ParagraphUser> userRepository, IRepository<Article> articleRepository, UserManager<ParagraphUser> userManager, IRepository<Request> requestRepository)
        {
            this.userRepository = userRepository;
            this.articleRepository = articleRepository;
            this.userManager = userManager;

        }

        public async Task<ParagraphUser>
            SetRandomAdmin()
        {
            var user = this.userRepository.All().OrderBy(p => Guid.NewGuid()).FirstOrDefault();

            var task = await userManager.AddToRoleAsync(user, "Admin");


            return user;
        }

        public UserDetailsModel Details(string username)
        {
            var user = this.userRepository.All().FirstOrDefault(p => p.UserName == username);

            var articles = this.articleRepository.All()
                .Where(p => p.Author.UserName == username)
                .To<ArticleIdAndName>()
                .ToArray();

            string role = "User";
            var userRoles = userManager.GetRolesAsync(user).Result;

            if (userRoles.Count() != 0)
            {
                if (userRoles.Contains("Admin"))
                {
                    role = "Admin";
                }
                else
                {
                    role = String.Join(", ", userRoles);
                }

            }

            var model = new UserDetailsModel
            {
                Username = username,
                Articles = articles,
                Role = role
            };

            return model;

        }



    }
}
