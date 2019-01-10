using System;
using System.Collections.Generic;
using System.Linq;
using Paragraph.Services.DataServices.Models.Comment;
using Xunit;
using Moq;
using Paragraph.Data;
using Paragraph.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Paragraph.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using Paragraph.Services.DataServices.Models.Article;
using Paragraph.Services.DataServices.Models.Home;
using Paragraph.Data.Models;

namespace Paragraph.Services.DataServices.Tests
{
    public class CommentServiceTests
    {

        private readonly ParagraphContext context;
        private readonly ICommentService commentService;
        private readonly IServiceProvider provider;


        public CommentServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ParagraphContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));

            services.AddScoped<ICommentService, CommentService>();         

            AutoMapperConfig.RegisterMappings(
            typeof(ArticleViewModel).Assembly,
            typeof(IndexArticleViewModel).Assembly
            );


            this.provider = services.BuildServiceProvider();
            this.context = provider.GetService<ParagraphContext>();
            this.commentService = provider.GetService<ICommentService>();
        }

        [Fact]
        public void TestIf_AddComment_InsertsACommentIntoTheDatabase()
        {
            this.AddsAUserAndAnArticleToTheDatabase();

            this.commentService.AddComment(new AddCommentModel(), "writer", 1);

            Assert.Single(this.context.Comment);


        }

        [Fact]
        public void Delete()
        {
            //this.AddsAUserAndAnArticleToTheDatabase();
            this.context.Comment.Add(new Comment
            {
                Id = 1,
            });

            this.context.SaveChanges();
            Assert.NotEmpty(this.context.Comment);

            this.commentService.Delete(1);

            Assert.Empty(this.context.Comment);

            
        }

        [Fact]
        public void DisplayComments()
        {
            this.AddsAUserAndAnArticleToTheDatabase();
            this.context.Comment.AddRange(new List<Comment>
            {
                new Comment{ Id = 1, ArticleId = 1},
                new Comment{ Id = 2, ArticleId = 1},
                new Comment{ Id = 3, ArticleId = 1},
                new Comment{ Id = 4, ArticleId = 1},
            });

            this.context.SaveChanges();

            Assert.Equal(4, this.commentService.DisplayComments(1).Count());
        }

        [Fact]
        public void DoesCommentExist()
        {
            this.context.Add(new Comment
            {
                Id = 1
            });

            this.context.SaveChanges();

            Assert.True(this.commentService.DoesCommentExist(1));
            Assert.False(this.commentService.DoesCommentExist(2));
        }

        private void AddsAUserAndAnArticleToTheDatabase()
        {
            this.context.Articles.Add(new Data.Models.Article
            {
                Id = 1
            });
            this.context.Users.Add(new ParagraphUser
            {
                UserName = "writer"
            });

            this.context.SaveChanges();
        }
    }
}
