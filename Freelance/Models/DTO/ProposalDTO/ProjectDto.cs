
namespace Freelance.Models.DTO.ProposalDTO
{
	public class ProjectDto
	{
		public Guid ProjectId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public double Budget { get; set; }
		public string Author { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
