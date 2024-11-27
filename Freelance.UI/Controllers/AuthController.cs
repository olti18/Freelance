using Freelance.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace Freelance.UI.Controllers
{
	public class AuthController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public AuthController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
		
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginRequestDto model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var httpClient = _httpClientFactory.CreateClient("AuthApiClient");

			var jsonContent = JsonSerializer.Serialize(model);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			var response = await httpClient.PostAsync("/api/Auth/Login", content);

			if (response.IsSuccessStatusCode)
			{
				var responseData = await response.Content.ReadAsStringAsync();
				var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(responseData);

				TempData["JwtToken"] = loginResponse.JwtToken;
				return RedirectToAction("Index", "Home"); // Redirect to home/dashboard
			}

			ModelState.AddModelError("", "Invalid username or password.");
			return View(model);
		}
	}
}
