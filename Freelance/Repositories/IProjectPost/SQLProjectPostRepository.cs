using Freelance.Data;
using Freelance.Models.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Freelance.Repositories.IProjectPost
{
	public class SQLProjectPostRepository : IProjectPostRepository
    {
        private readonly FreelanceDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        public SQLProjectPostRepository(FreelanceDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;

		}


		public async Task<ProjectPost> CreateAsync(ProjectPost projectPost)
        {
            await context.AddAsync(projectPost);
            await context.SaveChangesAsync();
            return projectPost;
        }

        public async Task<ProjectPost> DeleteAsync(Guid id)
        {
            var existingProject = await context.ProjectPosts.FirstOrDefaultAsync(x => x.Id == id);
                  context.ProjectPosts.Remove(existingProject);
            await context.SaveChangesAsync();
            return existingProject;
        }

		public async Task<List<ProjectPost?>> GetMyProjectsAsync(string userId)
		{
			if (string.IsNullOrEmpty(userId))
			{
				throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
			}

			var myProjects = context.ProjectPosts
	            .Where(p => p.UserId == userId) // Filter by UserId
	            .Include(p => p.Proposals) // Include related Proposals
	            .ThenInclude(pr => pr.Freelancer) // Include Freelancer for each proposal
	            .ToList();
            return myProjects;
        }

		public async Task<List<ProjectPost>> GetProjectsAsync()
		{
			var AllProjects = context.ProjectPosts
                .ToList();

			return AllProjects;
		}

		public async Task<ProjectPost?> UpdateAsync(Guid id, ProjectPost? projectPost)
        {
            var existingProject = await context.ProjectPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (existingProject == null)
            {
                return null;
            }

            existingProject.Title = projectPost.Title;
            existingProject.Description = projectPost.Description;
            existingProject.Author = projectPost.Author;
            existingProject.Budget = projectPost.Budget;
            existingProject.CreatedDate = projectPost.CreatedDate;

            await context.SaveChangesAsync();
            return existingProject;
        }
    }
}
