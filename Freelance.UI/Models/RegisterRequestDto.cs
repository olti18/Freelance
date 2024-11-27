namespace Freelance.UI.Models
{
	public class RegisterRequestDto
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public List<string> Roles { get; set; }
	}
}
