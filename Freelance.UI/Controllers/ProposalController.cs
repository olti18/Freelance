using Freelance.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Freelance.UI.Controllers
{
	public class ProposalController : Controller
	{
			private readonly IHttpClientFactory _httpClientFactory;

			public ProposalController(IHttpClientFactory httpClientFactory)
			{
				_httpClientFactory = httpClientFactory;
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
						return RedirectToAction("Index", "Projects");
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
			}
		
	}

