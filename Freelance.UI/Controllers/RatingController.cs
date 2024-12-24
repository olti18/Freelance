
using Freelance.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;

namespace Freelance.UI.Controllers
{
	public class RatingController : Controller
	{
		//public IActionResult Index()
		//{
		//	return View();
		//}
		private readonly IHttpClientFactory _httpClientFactory;

		public RatingController(IHttpClientFactory _httpClientFactory)
		{
			this._httpClientFactory = _httpClientFactory;
		}

		[HttpPost("Proposalss/RateFreelancer")]
		public async Task<IActionResult> RateFreelancer(AddRatingDto ratingDto)
		{
			var jwtToken = HttpContext.Request.Cookies["JwtToken"];
			if (string.IsNullOrEmpty(jwtToken))
			{
				TempData["ErrorMessage"] = "You must be logged in to rate a freelancer.";
				return RedirectToAction("Login", "Auth");
			}

			try
			{
				// Create HTTP client
				var client = _httpClientFactory.CreateClient();
				client.DefaultRequestHeaders.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

				
				// Send POST request to the API
				//var response = await client.PostAsJsonAsync("https://localhost:7086/api/Proposal/rate-freelancer", ratingDto);
				var response = await client.PostAsJsonAsync("https://localhost:7086/api/Rating/rate-freelancer", ratingDto);


				if (response.IsSuccessStatusCode)
				{
					TempData["SuccessMessage"] = "Rating submitted successfully.";
				}
				else
				{
					var error = await response.Content.ReadAsStringAsync();
					TempData["ErrorMessage"] = $"Failed to submit rating: {error}";
				}
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
			}

			//return RedirectToAction("index");
			return RedirectToAction("index", "ProjectPost");
		}








		//[HttpPost("rate-freelancer/{projectPostId}/{proposalId}")]
		//public async Task<IActionResult> RateFreelancer(Guid projectPostId, Guid proposalId)
		//{
		//	var message = string.Empty;

		//	// Read JWT token from cookies
		//	var jwtToken = HttpContext.Request.Cookies["JwtToken"];
		//	if (string.IsNullOrEmpty(jwtToken))
		//	{
		//		TempData["ErrorMessage"] = "You are not logged in. Please log in to continue.";
		//		return RedirectToAction("Login", "Auth");
		//	}

		//	try
		//	{
		//		// Create an HttpClient instance
		//		var client = _httpClientFactory.CreateClient();

		//		// Add JWT token as Bearer in the header
		//		client.DefaultRequestHeaders.Authorization =
		//			new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

		//		// Call the RateFreelancer API endpoint
		//		//var responseMessage = await client.PostAsync($"https://localhost:7086/api/Proposal/rate-freelancer?projectPostId={projectPostId}&proposalId={proposalId}", null);

		//		var responseMessage = await client.PostAsync($"https://localhost:7150/api/Rating/rate-freelancer?projectPostId={projectPostId}&proposalId={proposalId}", null);



		//		// Check if the response is successful
		//		if (responseMessage.IsSuccessStatusCode)
		//		{
		//			message = "Freelancer rating process initiated successfully!";
		//			TempData["SuccessMessage"] = message;
		//		}
		//		else
		//		{
		//			// Read error response from API
		//			var error = await responseMessage.Content.ReadAsStringAsync();
		//			message = $"Failed to rate freelancer: {error}";
		//			TempData["ErrorMessage"] = message;
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		// Handle exceptions
		//		message = $"An error occurred during the process: {ex.Message}";
		//		TempData["ErrorMessage"] = message;
		//	}

		//	// Redirect to the proposals page
		//	return RedirectToAction("MyProposals");
		//}

		//[HttpPost("rate-freelancer/{projectPostId}/{proposalId}")]
		//public async Task<IActionResult> RateFreelancer(Guid projectPostId, Guid proposalId)
		//{
		//	var message = string.Empty;

		//	var jwtToken = HttpContext.Request.Cookies["JwtToken"];
		//	if (string.IsNullOrEmpty(jwtToken))
		//	{
		//		TempData["ErrorMessage"] = "You are not logged in. Please log in to continue.";
		//		return RedirectToAction("Login", "Auth");
		//	}

		//	try
		//	{
		//		var client = _httpClientFactory.CreateClient();
		//		client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

		//		var responseMessage = await client.PostAsync($"https://localhost:7150/api/Rating/rate-freelancer?projectPostId={projectPostId}&proposalId={proposalId}", null);

		//		if (responseMessage.IsSuccessStatusCode)
		//		{
		//			message = "Freelancer rating process initiated successfully!";
		//			TempData["SuccessMessage"] = message;
		//		}
		//		else
		//		{
		//			var error = await responseMessage.Content.ReadAsStringAsync();
		//			message = $"Failed to rate freelancer: {error}";
		//			TempData["ErrorMessage"] = message;
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		message = $"An error occurred during the process: {ex.Message}";
		//		TempData["ErrorMessage"] = message;
		//	}

		//	return RedirectToAction("MyProposals");
		//}


		//public async Task<IActionResult> SubmitRating(AddRatingDto ratingDto)
		//{
		//	var jwtToken = HttpContext.Request.Cookies["JwtToken"];
		//	if (string.IsNullOrEmpty(jwtToken))
		//	{
		//		TempData["ErrorMessage"] = "Please log in to continue.";
		//		return RedirectToAction("Login", "Auth");
		//	}

		//	try
		//	{
		//		var client = _httpClientFactory.CreateClient();

		//		// Add Authorization Header
		//		client.DefaultRequestHeaders.Authorization =
		//			new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

		//		// Prepare the request
		//		var jsonContent = System.Text.Json.JsonSerializer.Serialize(ratingDto);
		//		var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

		//		// Send POST request
		//		//var response = await client.PostAsync("https://localhost:7150/Proposal/RateFreelancer", httpContent);
		//		var response = await client.PostAsync("https://localhost:7150/api/Rating/rate-freelancer", httpContent);

		//		if (response.IsSuccessStatusCode)
		//		{
		//			TempData["SuccessMessage"] = "Rating submitted successfully!";
		//		}
		//		else
		//		{
		//			var error = await response.Content.ReadAsStringAsync();
		//			TempData["ErrorMessage"] = $"Failed to submit rating: {error}";
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
		//	}

		//	return RedirectToAction("MyProposals");
		//}




	}
}
