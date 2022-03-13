using Microsoft.EntityFrameworkCore;

namespace ParameterTampering.Entities
{
    public class StoreDBContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                                    => optionsBuilder.UseSqlite("Data Source=StoreDB.db");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Article>().ToTable("Articles");
            builder.Entity<Comment>().ToTable("Comments");

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
