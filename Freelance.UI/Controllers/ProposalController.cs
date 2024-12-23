using Freelance.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Freelance.UI.Controllers
{
	public class ProposalController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public ProposalController(IHttpClientFactory _httpClientFactory)
		{
			this._httpClientFactory = _httpClientFactory;
		}

		[HttpGet("Proposalss/MyProposalss")]
		public async Task<IActionResult> MyProposals()
		{
			List<ProjectWithProposalsDto> response = new List<ProjectWithProposalsDto>();

			// Retrieve JWT token from cookies (if needed for authentication)
			var jwtToken = HttpContext.Request.Cookies["JwtToken"];
			if (string.IsNullOrEmpty(jwtToken))
			{
				return RedirectToAction("Login", "Auth");
			}

			//try
			//{
				// Create an HTTP client
				var client = _httpClientFactory.CreateClient();

				// Add Authorization header with Bearer token
				client.DefaultRequestHeaders.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

				// Call the API endpoint
				var responseMessage = await client.GetAsync("https://localhost:7086/api/Proposal/GetAllProposals");

				// Ensure the response status is success
				if (!responseMessage.IsSuccessStatusCode)
				{
					TempData["ErrorMessage"] = "Failed to load your proposals. Please try again.";
					return View(response); // Return an empty list
				}

				// Deserialize the JSON response into a List<ProjectWithProposalsDto>
				response = await responseMessage.Content.ReadFromJsonAsync<List<ProjectWithProposalsDto>>();
			//}
			//catch (Exception)
			//{
			//	TempData["ErrorMessage"] = "An error occurred while loading your proposals.";
			//}

			return View(response);
		}

		
		// Show the Create Proposal page
		[HttpGet]
		public IActionResult CreateProposal(Guid projectPostId)
		{
			// Pass the project ID to the view
			var model = new AddProposalDto
			{
				ProjectPostId = projectPostId
			};
			return View(model);
		}

		// Handle Proposal Submission
		[HttpPost]
		public async Task<IActionResult> SubmitProposal(AddProposalDto proposalDto)
		{
			var jwtToken = HttpContext.Request.Cookies["JwtToken"];
			if (string.IsNullOrEmpty(jwtToken))
			{
				return RedirectToAction("Login", "Auth"); // Redirect to login if no token
			}
			var client = _httpClientFactory.CreateClient();

			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

			if (proposalDto == null || string.IsNullOrEmpty(proposalDto.Content) || proposalDto.ProposedAmount <= 0)
			{
				TempData["Error"] = "All fields are required.";
				return View("CreateProposal", proposalDto);
			}

			try
			{

				var response = await client.PostAsJsonAsync("https://localhost:7086/api/Proposal", proposalDto);

				if (response.IsSuccessStatusCode)
				{
					TempData["Success"] = "Proposal created successfully!";
					return RedirectToAction("Index", "Projectpost");
				}
				else
				{
					var errorMessage = await response.Content.ReadAsStringAsync();
					TempData["Error"] = $"Failed to create proposal: {errorMessage}";
					return View("CreateProposal", proposalDto);
				}
			}
			catch (Exception ex)
			{
				TempData["Error"] = $"An error occurred: {ex.Message}";
				return View("CreateProposal", proposalDto);
			}
		}



		[HttpPost("Proposalss/AcceptMyProposal/{proposalId}")]
		public async Task<IActionResult> AcceptMyProposal(Guid proposalId)
		{
			// Lista për mesazhet e gabimeve dhe suksesit
			var message = string.Empty;

			// Lexo token-in JWT nga cookies
			var jwtToken = HttpContext.Request.Cookies["JwtToken"];
			if (string.IsNullOrEmpty(jwtToken))
			{
				TempData["ErrorMessage"] = "Ju nuk jeni i kyçur. Ju lutemi kyçuni për të vazhduar.";
				return RedirectToAction("Login", "Auth");
			}

			try
			{
				// Krijo një instancë të HttpClient
				var client = _httpClientFactory.CreateClient();

				// Shto token-in JWT si Bearer në header
				client.DefaultRequestHeaders.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

				// Thirr endpoint-in AcceptProposal
				var responseMessage = await client.PostAsync($"https://localhost:7086/api/Proposal/accept-proposal/{proposalId}", null);

				// Kontrollo nëse përgjigjja është e suksesshme
				if (responseMessage.IsSuccessStatusCode)
				{
					message = "Propozimi u pranua me sukses!";
					TempData["SuccessMessage"] = message;
				}
				else
				{
					// Lexo përgjigjen e gabimit nga API
					var error = await responseMessage.Content.ReadAsStringAsync();
					message = $"Dështoi pranimi i propozimit: {error}";
					TempData["ErrorMessage"] = message;
				}
			}
			catch (Exception ex)
			{
				// Kap dhe trajto gabimet
				message = $"Një gabim ndodhi gjatë procesit: {ex.Message}";
				TempData["ErrorMessage"] = message;
			}

			// Ridrejto te një pamje që tregon rezultatin
			return RedirectToAction("MyProposals");
		}






		//[HttpPost("Proposalss/{proposalId}")]
		////[HttpPost("Proposalss/{proposalId}")]
		//public async Task<IActionResult> AcceptProposal(Guid proposalId)
		//{
		//	// Retrieve JWT token from cookies
		//	var jwtToken = HttpContext.Request.Cookies["JwtToken"];
		//	if (string.IsNullOrEmpty(jwtToken))
		//	{
		//		return RedirectToAction("Login", "Auth");
		//	}

		//	try
		//	{
		//		// Create an HTTP client
		//		var client = _httpClientFactory.CreateClient();

		//		// Add Authorization header with Bearer token
		//		client.DefaultRequestHeaders.Authorization =
		//			new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

		//		//Call the API endpoint
		//		//var responseMessage = await client.PostAsync(
		//		//	$"https://localhost:7086/api/Proposalss/accept-proposal/{proposalId}",
		//		//	null // No body is required for this POST request
		//		//);
		//		//var responseMessage = await client.PostAsync("https://localhost:7086/api/Proposal/accept-proposal/id", null);


		//		var responseMessage = await client.PostAsync(
		//   $"https://localhost:7086/api/accept-proposal/{proposalId}", null); // Updated URL

		//		// Ensure the response status is success
		//		if (!responseMessage.IsSuccessStatusCode)
		//		{
		//			TempData["ErrorMessage"] = "Failed to accept the proposal. Please try again.";
		//			return RedirectToAction("MyProposals");
		//		}

		//		TempData["SuccessMessage"] = "Proposal accepted successfully.";
		//	}
		//	catch (Exception)
		//	{
		//		TempData["ErrorMessage"] = "An error occurred while accepting the proposal.";
		//	}

		//	return RedirectToAction("MyProposals");
		//}


	}
}
	

