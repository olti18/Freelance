using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Freelance.Models.Domain
{
    public class ProjectPost
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
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
