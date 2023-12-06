using Entities;
using Entities.CategorySystem;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class DBConnector : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryCollection> CategoryCollections { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<ImageCollection> ImageCollections { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<SalePost> SalePosts { get; set; }
        public DbSet<SocialMediaPost> SocialMediaPosts { get; set; }
        public DbSet<User> Users { get; set; }
        
        public DBConnector(DbContextOptions<DBConnector> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=sqlite3.db");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure SalePost enum to be stored as int
            modelBuilder
                .Entity<SalePost>()
                .Property(p => p.PostType)
                .HasConversion<int>();

            // To set default value for datetime
            // modelBuilder
            //    .Entity<Post>()
            //    .Property(p => p.PostTime)
            //    .HasDefaultValueSql("DATETIME('NOW')"); // Change to NOW(); for MySQL

            base.OnModelCreating(modelBuilder);
        }
    }
}