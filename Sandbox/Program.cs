using System;
using System.IO;
using System.Linq;
using System.Text;
using Paragraph.Data;
using Paragraph.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Paragraph.Data.Common;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace Sandbox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine($"{typeof(Program).Namespace} ({string.Join(" ", args)}) starts working...");

            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);


            using (var serviceScope = serviceProvider.CreateScope())
            {
                serviceProvider = serviceScope.ServiceProvider;

              SandboxCode(serviceProvider);
               
            }
        }

        private static void SandboxCode(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ParagraphContext>();

            var categories = SeedCategories();
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var articles = ReadArticles();

            var rnd = new Random();

            var author = context.Users.FirstOrDefault();
            foreach (var article in articles)
            {
                int categoryId = rnd.Next(1, context.Categories.Count() - 1);
                article.Category = context.Categories.Find(categoryId);
                article.Author = author;

            }

            context.Articles.AddRange(articles);
            context.SaveChanges();

        }

        private static Category[] SeedCategories()
        {
            string[] categoryNames = new string[]
            {

                "Character writing",
                "World Building",
                "Genre",
                "How to-s",
                "Interview",
                "News"
            };

            var categories = new Category[6];

            for (int i = 0; i < 6; i++)
            {
                var category = new Category
                {
                    Name = categoryNames[i]
                };


                categories[i] = category;

            }


            return categories;

        }

        private static Article[] ReadArticles()
        {

            string path = Directory.GetCurrentDirectory() + "//articles.txt";
            var text = File.ReadAllLines(path);
            var articles = new Article[13];
            int index = 0;
            for (int i = 0; i < text.Length - 1; i++)
            {
                if (String.IsNullOrWhiteSpace(text[i]))
                {
                    continue;
                }
                string title = Regex.Replace(text[i], @"[^\u0000-\u007F]+", string.Empty); 
                string content = Regex.Replace(text[i + 1], @"[^\u0000-\u007F]+", string.Empty);

                var article = new Article
                {
                    Title = title,
                    Content = content
                };

                articles[index] = article;
                i++;
                index++;

            }
            return articles;
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            services.AddDbContext<ParagraphContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
            services.AddScoped<UserManager<ParagraphUser>>();
            services.AddScoped<RoleManager<IdentityRole>>();


        }
    }
}
