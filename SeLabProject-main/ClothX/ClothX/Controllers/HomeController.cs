using ClothX.CustomAttributes;
using ClothX.DbModels;
using ClothX.Models;
using ClothX.Services;
using ClothX.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Import the ILogger namespace
using System.Diagnostics;
using System.Threading.Tasks; // Import the Task namespace
using System.Collections.Generic;
using System.Linq;

namespace ClothX.Controllers
{
	// Controller for managing the home page and related functionalities
	public class HomeController : Controller
	{
		// Private field to store the logger
		private readonly ILogger<HomeController> _logger;

		// Constructor to initialize the logger
		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		// Action to display the home page
		public async Task<IActionResult> Index()
		{
			// Retrieve and display a list of active tailor projects
			List<TailorProject> tailorProjects = new List<TailorProject>();
			try
			{
				tailorProjects = await TailorProjectsUtility.Instance.getTailorProjects();
				tailorProjects = tailorProjects
					.Where(x => x.IsActive == true)
					.OrderByDescending(obj => obj.AddedOn)
					.Take(3)
					.ToList();
			}
			catch (Exception ex)
			{
				// Log any exceptions and continue
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}

			// Pass the tailor projects to the view
			ViewBag.TailorProjects = tailorProjects;
			return View();
		}

		// Action to display the services page
		public IActionResult Services()
		{
			// Display the services page
			return View();
		}

		// Action to display feedback (requires permission to add feedback)
		[ClothXPermissionAuthorize("Feedback.Add")]
		public IActionResult Feedback(string url)
		{
			// Pass the current URL to the feedback partial view
			ViewData["CurrentUrl"] = url;
			return PartialView("FeedbackPartialView");
		}

		// Action to handle errors
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			// Display the error view with the error details
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
