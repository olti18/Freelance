namespace Freelance.UI.Models
{
	public class ProjectWithProposalsDto
	{

		public Guid ProjectId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Author { get; set; }
		public decimal Budget { get; set; }
		public DateTime CreatedDate { get; set; }
		public List<ProposalDto> Proposals { get; set; }


		/*public int ProjectId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Author { get; set; }
		public decimal Budget { get; set; }
		public DateTime CreatedDate { get; set; }
		public List<Proposal> Proposals { get; set; }*/

	}
}
