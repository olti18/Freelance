using Freelance.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Freelance.UI.Controllers
{
	public class ProjectPostController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public ProjectPostController(IHttpClientFactory _httpClientFactory)
		{
			this._httpClientFactory = _httpClientFactory;
		}

		/*[HttpGet]
		public async Task<IActionResult> GetAllProjects()
		{
			// Retrieve the token from cookies
			var jwtToken = HttpContext.Request.Cookies["JwtToken"];
			if (string.IsNullOrEmpty(jwtToken))
			{
				return RedirectToAction("Login", "Account"); // Redirect to login if no token
			}

			var httpClient = _httpClientFactory.CreateClient();

			// Add the Authorization header
			httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

			// Call the protected API endpoint
			//var response = await httpClient.GetAsync("https://localhost:7086/api/ProjectPost/GetAllProjects");
			var response = await httpClient.GetAsync("https://localhost:7086/api/projectpost");

			if (response.IsSuccessStatusCode)
			{
				// Deserialize the API response
				var responseData = await response.Content.ReadAsStringAsync();
				var projects = System.Text.Json.JsonSerializer.Deserialize<List<ProjectPostDto>>(responseData);

				return View(projects); // Pass data to the view
			}

			ModelState.AddModelError("", "Failed to fetch projects.");
			return View(new List<ProjectPostDto>());
		}*/


		[HttpGet]
		public async Task<IActionResult> Index()
		{
			List<ProjectPostDto> response = new List<ProjectPostDto>();
			var jwtToken = HttpContext.Request.Cookies["JwtToken"];
			if (string.IsNullOrEmpty(jwtToken))
			{
				return RedirectToAction("Login", "Account"); // Redirect to login if no token
			}
			var client = _httpClientFactory.CreateClient();
			//var httpClient = _httpClientFactory.CreateClient();

			// Add the Authorization header
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

			try
			{
				//var client = _httpClientFactory.CreateClient();
				var httpResonseMessage = await client.GetAsync("https://localhost:7086/api/ProjectPost");

				httpResonseMessage.EnsureSuccessStatusCode();

				response.AddRange(await httpResonseMessage.Content.ReadFromJsonAsync<IEnumerable<ProjectPostDto>>());
			}catch (Exception ex)
			{

			}
			return View(response);
			// Call the Web API to get the project posts
			//var response = await client.GetAsync("https://localhost:7086/api/projectpost");

			//if (response.IsSuccessStatusCode)
			//{
			//	// Deserialize the JSON response into a List of ProjectPostDto objects
			//	var json = await response.Content.ReadAsStringAsync();
			//	var projectPosts = JsonSerializer.Deserialize<List<ProjectPostDto>>(json);

			//	// Pass the project posts to the view
			//	return View(projectPosts);
			//}
			//else
			//{
			//	// Handle the error or show an error page
			//	return View("Error");
			//}

		}
	}
}
