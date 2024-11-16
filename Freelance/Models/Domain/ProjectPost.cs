using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

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
        

    }
}
