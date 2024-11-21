using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Freelance.Models.Domain
{
	public class Proposal
	{
		public Guid Id { get; set; }
		public string Content { get; set; }
		public decimal ProposedAmount { get; set; }
		public DateTime SubmittedDate { get; set; }
		public DateTime CreatedDate { get; set; }

		public Guid ProjectPostId { get; set; } // Foreign Key to JobPost
		[JsonIgnore]
		public ProjectPost ProjectPost { get; set; }
		// Foreign Key 
		public string FreelancerId { get; set; } // Foreign key
		public IdentityUser Freelancer { get; set; } // Navigation property

		public bool IsSelected { get; set; } = false;
		
	}
}
