
namespace Freelance.Models.DTO.ProposalDTO
{
	public class ProjectWithProposalsDto
	{
		public Guid ProposalId { get; set; }
		public decimal ProposedAmount { get; set; }
		public string Content { get; set; }
		public DateTime SubmittedDate { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsSelected { get; set; }
		public ProjectDto Project { get; set; }
	}
}
