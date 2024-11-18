namespace Freelance.Models.DTO.ProposalDTO
{
	public class AddProposalDto
	{
		public Guid ProjectPostId { get; set; }

		public string Content { get; set; }
		public decimal ProposedAmount { get; set; }
		public DateTime CreatedDate { get; set; }

	}
}
