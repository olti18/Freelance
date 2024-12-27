using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Freelance.Models.Domain
{
    public class ProjectPost
    {
        public Guid Id { get; set; }

		[Required(ErrorMessage = "Title is required.")]
		public string Title { get; set; }

		[Required(ErrorMessage = "Description is required.")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Author is required.")]
		[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Author can only contain letters and spaces.")]
		public string Author { get; set; }

		[Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive value.")]
		public Double Budget { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        // Foreign Key 
        public IdentityUser User { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

		[JsonIgnore] // Loop in DB
		public ICollection<Proposal> Proposals { get; set; }

		public Guid? SelectedProposalId { get; set; } // Nullable in case no one is assigned yet
		public Proposal SelectedProposal { get; set; } // Navigation property
		public ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();


	}
}
