using Freelance.Data;
using Freelance.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Freelance.Repositories.IProjectPost
{
    public class SQLProjectPostRepository : IProjectPostRepository
    {
        private readonly FreelanceDbContext context;
        public SQLProjectPostRepository(FreelanceDbContext context)
        {
            this.context = context;
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
