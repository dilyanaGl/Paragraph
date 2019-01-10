using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Paragraph.Data;
using Paragraph.Data.Common;
using Paragraph.Data.Models;
using Paragraph.Services.DataServices.Models.Article;
using Paragraph.Services.DataServices.Models.Home;
using Paragraph.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace Paragraph.Services.DataServices.Tests
{
    public class RequestServiceTests
    {
        private readonly ParagraphContext context;
        private readonly IRequestService requestService;
        private readonly IServiceProvider provider;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ParagraphUser> userManager;


        public RequestServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ParagraphContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<UserManager<ParagraphUser>>();
            services.AddScoped<RequestService, RequestService>();
            services.AddIdentity<ParagraphUser, IdentityRole>(
                options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.User.RequireUniqueEmail = true;


                })
                .AddEntityFrameworkStores<ParagraphContext>()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders();

            services.AddScoped<RoleManager<IdentityRole>>();

            AutoMapperConfig.RegisterMappings(
            typeof(ArticleViewModel).Assembly,
            typeof(IndexArticleViewModel).Assembly
            );


            this.provider = services.BuildServiceProvider();
            this.context = provider.GetService<ParagraphContext>();
            this.requestService = provider.GetService<IRequestService>();
            this.roleManager = provider.GetService<RoleManager<IdentityRole>>();

            this.userManager = provider.GetService<UserManager<ParagraphUser>>();
        }

        [Fact]
        public void TestIf_GetAdminRequests_ReturnsCorrectNumberOfRoles()
        {
            SetDatabaseForGetUserAndAdminRequests();
            Assert.Equal(3, this.requestService.GetAdminRequests("Solomon").Count());
            Assert.Empty(this.requestService.GetAdminRequests("Solomon").Where(p => p.Username == "Solomon"));

        }
        private void SetDatabaseForGetUserAndAdminRequests()
        {
            var user = new ParagraphUser { Id = "solomon", UserName = "Solomon" };
            this.context.Users.Add(user);
            this.context.SaveChanges();
            var role = new IdentityRole { Name = "Admin" };
            this.roleManager.CreateAsync(role);
            this.userManager.AddToRoleAsync(user, "Admin");

            var users = new List<ParagraphUser>()
            {
                new ParagraphUser { Id = "simon", UserName = "Simon" },
                new ParagraphUser { Id = "salambo", UserName = "Salambo" },
                new ParagraphUser { Id = "sinbad", UserName = "Sinbad" }
            };

            this.context.Users.AddRange(users);
            this.context.SaveChanges();

            foreach (var u in users)
            {
                this.context.Requests.Add(new Request
                {
                    RequestSenderId = u.Id,
                    Role = role,
                    RequestReceiverId = user.Id
                });
                this.context.SaveChanges();
            }
        }

        [Fact]
        public void TestIf_GetAllRoles_ReturnsCorrectNumberOfRoles()
        {
            var roles = new List<IdentityRole>()
            {
                new IdentityRole{Name = "Writer"},
                new IdentityRole{Name = "User"},
                new IdentityRole{ Name = "Admin"}
            };

            roles.ForEach(p => roleManager.CreateAsync(p));
            var user = new ParagraphUser { UserName = "Solomon" };

            this.context.Users.Add(user);
            this.context.SaveChanges();


            Assert.Single(this.requestService.GetAllRoles("Solomon"));
            Assert.Contains("Writer", this.requestService.GetAllRoles("Solomon"));

            foreach (var role in roles)
            {
                this.context.Requests.Add(new Request
                {
                    Role = role,
                    RequestSender = user
                });
            }

            this.context.SaveChanges();


            Assert.Empty(this.requestService.GetAllRoles("Solomon"));
        }

        [Fact]
        public void TestIf_GetRole_ReturnsCorrectRole()
        {
            SetRequestForUsernameAndRole();

            Assert.Matches("Admin", this.requestService.GetRole(1));
        }

        private void SetRequestForUsernameAndRole()
        {
            this.roleManager.CreateAsync(new IdentityRole
            {
                Id = "admin",
                Name = "Admin"
            });

            var users = new List<ParagraphUser>
            {
                new ParagraphUser
                {
                    Id = "alibaba",
                    UserName = "Ali Baba"
                },
                new ParagraphUser
                {
                    Id = "aladin",
                    UserName = "Aladin"
                }
            };

            this.context.Users.AddRange(users);

            this.context.SaveChanges();

            this.context.Requests.Add(new Request
            {
                Id = 1,
                RequestReceiverId = "aladin",
                RequestSenderId = "alibaba",
                RoleId = "admin"
            });

            this.context.SaveChanges();


        }

        [Fact]
        public void TestIf_GetUserId_ReturnsCorrectUsername()
        {
            this.SetRequestForUsernameAndRole();
            Assert.Matches("alibaba", this.requestService.GetUserId(1));

        }

        [Fact]
        public void TestIf_GetUserRequests_ReturnsCorrectNumberOfRoles()
        {
            SetDatabaseForGetUserAndAdminRequests();
            Assert.Single(this.requestService.GetUserRequests("Sinbad"));
            Assert.Single(this.requestService.GetUserRequests("Salambo"));
        }


        [Fact]
        public void TestIf_MakeRequest_AddsARequestToTheDatabase()
        {
            MakeRequest();
            //var role = new IdentityRole
            //{
            //    Name = "User"
            //};
            //this.roleManager.CreateAsync(role);

            //this.context.Users.Add(new ParagraphUser
            //{
            //    UserName = "Bambi",

            //});
            //this.context.SaveChanges();
            //this.requestService.MakeRequest("Bambi", role);

            Assert.Single(context.Requests);


        }

        private void MakeRequest()
        {
            var role = new IdentityRole
            {
                Name = "User"
            };
            this.roleManager.CreateAsync(role);

            this.context.Users.Add(new ParagraphUser
            {
                UserName = "Bambi",

            });
            this.context.SaveChanges();
            this.requestService.MakeRequest("Bambi", role);
        }

        [Fact]
        public void TestIf_SetRequestStatus_SetsStatusToApproved()
        {
            MakeRequest();
            var requestId = this.context.Requests.Where(p => p.RequestSender.UserName == "Bambi").FirstOrDefault().Id;

            this.requestService.SetRequestStatus(requestId);

            Assert.Matches(Status.Approved.ToString(), this.context.Requests.FirstOrDefault(p => p.RequestSender.UserName == "Bambi").Status.ToString());

        }
    }
}
