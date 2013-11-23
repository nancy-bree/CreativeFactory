using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreativeFactory.Entities;

namespace CreativeFactory.DAL
{
    public class CreativeFactoryContext : DbContext
    {
        public DbSet<Article> Article { get; set; }

        public DbSet<Item> Item { get; set; }

        public DbSet<Rating> Rating { get; set; }

        public DbSet<Tag> Tag { get; set; }

        public DbSet<User> User { get; set; }

        public CreativeFactoryContext() : base(nameOrConnectionString: "CreativeFactoryContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasMany(x => x.Tags)
                .WithMany(y => y.Articles)
                .Map(x =>
                {
                    x.MapLeftKey("ArticleId");
                    x.MapRightKey("TagId");
                    x.ToTable("ArticleTag");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
