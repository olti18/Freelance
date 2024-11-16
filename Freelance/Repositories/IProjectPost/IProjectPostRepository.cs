using Freelance.Models.Domain;

namespace Freelance.Repositories.IProjectPost
{
    public interface IProjectPostRepository
    {
        Task<ProjectPost> CreateAsync(ProjectPost projectPost);
        Task<ProjectPost?> DeleteAsync(Guid id);
        Task<ProjectPost?> UpdateAsync(Guid id, ProjectPost? projectPost);    

    }
}
