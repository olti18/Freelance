using Freelance.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Freelance.Data
{
    public class FreelanceDbContext : IdentityDbContext
    {
        public FreelanceDbContext(DbContextOptions<FreelanceDbContext> options) : base(options)
        {
        }
        public DbSet<ProjectPost> ProjectPosts { get; set; }  
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = "262fe4f8-1a16-4022-896b-02a91ac72401";
            var userRoleId = "c104631c-600f-468b-8f00-daa624010de2";

            var roles = new List<IdentityRole>
            {


                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "User",
                    NormalizedName = "User".ToUpper(),
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);

        }
    }
}
