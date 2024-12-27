using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Freelance.Models.Domain
{
	public class Proposal
	{
		public Guid Id { get; set; }

		[Required(ErrorMessage = "Content is required.")]
		[StringLength(500, ErrorMessage = "Content cannot exceed 500 characters.")]
		public string Content { get; set; }

		[Required(ErrorMessage = "Proposed Amount is required.")]
		[Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Proposed Amount must be greater than 0.")]
		public decimal ProposedAmount { get; set; }

		[Required(ErrorMessage = "Submitted Date is required.")]
		[DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
		public DateTime SubmittedDate { get; set; }

		[Required(ErrorMessage = "Created Date is required.")]
		[DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
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
