using ClothX.CustomAttributes;
using ClothX.DbModels;
using ClothX.Services;
using ClothX.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ClothX.Controllers
{
	// Controller for managing feedback-related functionalities
	public class FeedbackController : Controller
	{
		// Action to handle the creation of feedback (requires permission to add feedback)
		[HttpPost]
		[ClothXPermissionAuthorize("Feedback.Add")]
		public async Task<IActionResult> Create(Review model, IFormCollection form)
		{
			try
			{
				// Access the database context
				ClothXDbContext db = new ClothXDbContext();

				// Get the user ID and set other properties of the feedback model
				int userId = UserUtility.Instance.getUserProfileId(User.Identity.Name);
				model.UserId = userId;
				model.AddedOn = DateTime.Now;
				model.AddedBy = User.Identity.Name;

				// Parse and set the rating from the form
				_ = int.TryParse(form["rate"], out int r);
				model.Rating = r;

				// Add the feedback to the database
				await db.Reviews.AddAsync(model);
				await db.SaveChangesAsync();

				// Redirect to the original URL
				return Redirect(form["CurrentUrl"]);
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to the server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to display a list of feedbacks (requires permission to manage feedback)
		[ClothXPermissionAuthorize("Feedback.Manage")]
		public async Task<IActionResult> Index()
		{
			try
			{
				// Access the database context
				ClothXDbContext db = new ClothXDbContext();

				// Retrieve and display a list of all feedbacks
				List<Review> reviews = await db.Reviews.ToListAsync();
				return View(reviews);
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to the server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to display information about a specific feedback (requires permission to manage feedback)
		[ClothXPermissionAuthorize("Feedback.Manage")]
		public async Task<IActionResult> Info(int Id)
		{
			try
			{
				// Access the database context
				ClothXDbContext db = new ClothXDbContext();

				// Pass the feedback ID to the view
				ViewBag.Id = Id;

				// Retrieve and display information about the specified feedback
				Review review = await db.Reviews.FindAsync(Id);
				return PartialView("FeedbackinfoPartialView", review);
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to the server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to respond to a specific feedback (requires permission to manage feedback)
		[ClothXPermissionAuthorize("Feedback.Manage")]
		public async Task<IActionResult> Respond(Review model)
		{
			try
			{
				// Access the database context
				ClothXDbContext db = new ClothXDbContext();

				// Retrieve the feedback from the database
				Review review = await db.Reviews.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

				// Update the response and response timestamp
				review.Response = model.Response;
				review.ResponseAddedOn = DateTime.Now;

				// Update the feedback in the database
				db.Reviews.Update(review);
				await db.SaveChangesAsync();

				// Send an email notification in response to feedback
				await MailSenderService.Instance.SendEmailOnFeedbackResponse(review);

				// Redirect to the feedback index page
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to the server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}
	}
}
