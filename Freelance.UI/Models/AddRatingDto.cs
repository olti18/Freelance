using System.ComponentModel.DataAnnotations;

namespace Freelance.UI.Models
{
	public class AddRatingDto
	{
			public Guid ProposalId { get; set; }
			public Guid ProjectPostId { get; set; }
			public Guid UserId { get; set; } // This is optional as it's inferred from the proposal in the API.
			public int Score { get; set; }
			public string? Comment { get; set; }
		

	}
}
