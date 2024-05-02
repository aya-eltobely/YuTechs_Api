using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace YuTechsTask.Models
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration config;

        public ApplicationDBContext() { }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options, IConfiguration _configuration) : base(options)
        {
            config = _configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(config.GetConnectionString("Default"));
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Image> Images { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configure Identity-related entities
            builder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            builder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
            builder.Entity<IdentityUserClaim<string>>().HasKey(c => c.Id);
            builder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            builder.Entity<IdentityRoleClaim<string>>().HasKey(rc => rc.Id);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///

            builder.Entity<Author>().HasMany(a => a.News).WithOne(n => n.Author).OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Image>().HasOne(i => i.News).WithOne(s => s.Image).HasForeignKey<News>(s => s.ImageId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
