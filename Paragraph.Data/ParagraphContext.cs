using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace Paragraph.Data
{
    using Paragraph.Data.Models;

    public class ParagraphContext : IdentityDbContext<ParagraphUser>
    {
        public ParagraphContext(DbContextOptions<ParagraphContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comment { get; set; }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<Request> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Request>()
                .HasOne(p => p.RequestReceiver)
                .WithMany(p => p.RequestsReceived)
                .HasForeignKey(p => p.RequestReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Request>()
                .HasOne(p => p.RequestSender)
                .WithMany(p => p.RequestsSent)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ArticleTag>()
                .HasOne(p => p.Article)
                .WithMany(p => p.Tags)
                .HasForeignKey(p => p.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ArticleTag>()
                .HasOne(p => p.Tag)
                .WithMany(p => p.ArticleTags)
                .HasForeignKey(p => p.TagId)
                .OnDelete(DeleteBehavior.Restrict);


           base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }


    }
}
