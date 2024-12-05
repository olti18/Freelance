using System.ComponentModel.DataAnnotations;

namespace Freelance.Models.DTO
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
		public string Role { get; set; } // Single role instead of a list
		//public string[] Roles { get; set; }
    }
}
