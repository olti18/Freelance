using Freelance.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Freelance.Data
{
    public class FreelanceDbContext : IdentityDbContext
    {
        public FreelanceDbContext(DbContextOptions<FreelanceDbContext> options) : base(options)
        {

        }
        public DbSet<ProjectPost> ProjectPosts { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
		public DbSet<Rating> Ratings { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

			// Rating -> User (Freelancer)
			builder.Entity<Rating>()
				.HasOne(r => r.User)
				.WithMany() // A User can receive many ratings (no navigation property back to Rating)
				.HasForeignKey(r => r.UserId)
				.OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

			// Rating -> JobPost
			builder.Entity<Rating>()
				.HasOne(r => r.ProjectPost)
				.WithMany(jp => jp.Ratings) // JobPost can have many Ratings
				.HasForeignKey(r => r.ProjectPostId)
				.OnDelete(DeleteBehavior.Cascade); // Optional: delete ratings if the JobPost is deleted
			
			//
			builder.Entity<ProjectPost>()
	          .HasOne(j => j.SelectedProposal)
	          .WithMany() // No back-reference in Proposal
	          .HasForeignKey(j => j.SelectedProposalId)
	          .OnDelete(DeleteBehavior.Restrict);

			builder.Entity<ProjectPost>()
		    .HasMany(pp => pp.Proposals) // Navigation property in ProjectPost
		    .WithOne(p => p.ProjectPost) // Navigation property in Proposal
		    .HasForeignKey(p => p.ProjectPostId) // Foreign key in Proposal
		    .OnDelete(DeleteBehavior.Restrict); // Optional: cascade delete proposals when ProjectPost is deleted

			// Configure ProjectPost -> User (Client)
			builder.Entity<ProjectPost>()
				.HasOne(pp => pp.User) // One Client per ProjectPost
				.WithMany() // A user can have multiple ProjectPosts
				.HasForeignKey(pp => pp.UserId)
				.OnDelete(DeleteBehavior.Restrict); // Prevent user deletion if ProjectPosts exist

			// Configure Proposal -> User (Freelancer)
			builder.Entity<Proposal>()
				.HasOne(p => p.Freelancer) // One Freelancer per Proposal
				.WithMany() // A user can have multiple Proposals
				.HasForeignKey(p => p.FreelancerId)
				.OnDelete(DeleteBehavior.Restrict); // Pre



			//
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
