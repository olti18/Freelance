namespace Freelance.Models.DTO.ProposalDTO
{
	public class ProposalDto
	{
		public Guid Id { get; set; }
		public string Description { get; set; }
		public decimal Budget { get; set; }
		public string Status { get; set; }
		public DateTime CreatedDate { get; set; }
		public Guid ProjectPostId { get; set; }
		public string FreelancerId { get; set; }

	}
}
