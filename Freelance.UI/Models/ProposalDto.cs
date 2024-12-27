namespace Freelance.UI.Models
{
	public class ProposalDto
	{
		
		public Guid ProposalId { get; set; }
		public string Content { get; set; }
		public decimal ProposedAmount { get; set; }
		public DateTime SubmittedDate { get; set; }
		public string FreelancerId { get; set; }
		public string FreelancerName { get; set; }
		public bool IsSelected { get; set; }
		public decimal Budget { get; set; }


		/*public int ProposalId { get; set; }
		public string Content { get; set; }
		public decimal ProposedAmount { get; set; }
		public DateTime SubmittedDate { get; set; }
		public string FreelancerId { get; set; }
		public string FreelancerName { get; set; }
		public bool IsSelected { get; set; }*/

	}
}
