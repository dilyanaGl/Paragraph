using System;
using System.Collections.Generic;
using System.Text;
using Paragraph.Data.Models;
using Paragraph.Services.DataServices.Models.User;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Paragraph.Data;
using Microsoft.EntityFrameworkCore;
using Paragraph.Services.DataServices.Models.Article;
using Paragraph.Services.DataServices.Models.Home;
using Paragraph.Services.Mapping;
using Paragraph.Data.Common;
using Microsoft.AspNetCore.Identity;


namespace Paragraph.Services.DataServices.Tests
{
    public class UserServiceTests 
    {
        private readonly ParagraphContext context;
        private readonly IUserService userService;
        private readonly IServiceProvider provider;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ParagraphUser> userManager;


        public UserServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ParagraphContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserManager<ParagraphUser>>();
           services.AddScoped<RequestService, RequestService>();
            services.AddIdentity<ParagraphUser, IdentityRole>(
                options => {
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
            this.userService = provider.GetService<IUserService>();
            this.userManager = provider.GetService<UserManager<ParagraphUser>>();
            this.roleManager = provider.GetService<RoleManager<IdentityRole>>();
        }


        [Fact]
        public void Details()
        {
           
            var users = new List<ParagraphUser>
            {
                new ParagraphUser{ UserName ="Salambo", Id = Guid.NewGuid().ToString(),  },
                new ParagraphUser{ UserName ="Sinbad", Id = Guid.NewGuid().ToString() },
                new ParagraphUser{ UserName ="Salamandar", Id = Guid.NewGuid().ToString() },
            };

            this.context.Users.AddRange(users);
            this.context.SaveChanges();
            var model = this.userService.Details("Salambo");

            Assert.IsType<UserDetailsModel>(model);
            Assert.Matches(model.Username, "Salambo");
          
                
        }     
    
    }
}
