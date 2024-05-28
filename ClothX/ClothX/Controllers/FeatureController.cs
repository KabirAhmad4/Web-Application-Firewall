using ClothX.CustomAttributes;
using ClothX.DbModels;
using ClothX.Services;
using ClothX.Utility;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg.Sig;

namespace ClothX.Controllers
{
	public class FeatureController : Controller
	{
		// Action to display the list of feature groups (requires permission to view feature groups)
		[ClothXPermissionAuthorize("FeatureGroups")]
		public IActionResult Index()
		{
			try
			{
				// Get the list of feature groups and display them
				List<FeatureGroup> features = FeaturesUtility.Instance.GetFeatures();
				return View(features);
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to the server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to create or edit a feature group (requires permission to manage feature groups)
		[ClothXPermissionAuthorize("FeatureGroups")]
		public IActionResult Create(int Id = 0)
		{
			try
			{
				// Pass the feature group ID to the view
				ViewBag.Id = Id;

				// If editing, retrieve the feature group and pass it to the view
				if (Id != 0)
				{
					FeatureGroup? feature = FeaturesUtility.Instance.FindFeatureGroup(Id);
					return PartialView("FeatureGroupCreateEditPartialView", feature);
				}

				// If creating, return an empty view
				return PartialView("FeatureGroupCreateEditPartialView");
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to the server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to handle the creation or editing of a feature group (requires permission to manage feature groups)
		[HttpPost]
		[ClothXPermissionAuthorize("FeatureGroups")]
		public IActionResult Create(FeatureGroup model)
		{
			try
			{
				// Check if creating or editing, and call the appropriate utility method
				if (model.Id == 0)
				{
					FeaturesUtility.Instance.AddFeatureGroup(model, User.Identity.Name);
				}
				else
				{
					FeaturesUtility.Instance.EditFeatureGroup(model);
				}

				// Redirect to the feature groups index page
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to the server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to delete a feature group (requires permission to manage feature groups)
		[ClothXPermissionAuthorize("FeatureGroups")]
		public async Task<IActionResult> Delete(int? id)
		{
			try
			{
				// Check if the ID is null, then call the utility method to delete the feature group
				if (id == null)
				{
					return NotFound();
				}

				await FeaturesUtility.Instance.DeleteFeatureGroup(id);

				// Redirect to the feature groups index page
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to the server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to display details of a specific feature group (requires permission to view features)
		[ClothXPermissionAuthorize("Feature")]
		public async Task<IActionResult> Details(int? id)
		{
			try
			{
				// Check if the ID is null, then retrieve and display information about the feature group
				if (id == null)
				{
					return NotFound();
				}

				var featureGroup = FeaturesUtility.Instance.FindFeatureGroup((int)id);

				// Check if the feature group is null, then return NotFound
				if (featureGroup == null)
				{
					return NotFound();
				}

				return View(featureGroup);
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to the server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to create or edit a feature (requires permission to manage features)
		[HttpGet]
		[ClothXPermissionAuthorize("Feature")]
		public IActionResult CreateFeature(string Id = "")
		{
			try
			{
				// Parse the feature group ID and feature ID from the provided string
				var ids = Id.Split(".");
				ViewBag.FeatureGroupId = Convert.ToInt32(ids[0]);
				ViewBag.Id = ids.Length == 2 ? Convert.ToInt32(ids[1]) : 0;

				// If editing, retrieve the feature and pass it to the view
				if (ids.Length == 2)
				{
					if (ids[1] != "0")
					{
						Feature? feature = FeaturesUtility.Instance.FindFeature(Convert.ToInt32(ids[1]));

						// Check if the feature is null, then return NotFound
						if (feature == null)
						{
							return NotFound();
						}

						return PartialView("FeatureCreateEditPartialView", feature);
					}


				}

				// If creating, return an empty view
				return PartialView("FeatureCreateEditPartialView");
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to the server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to handle the creation or editing of a feature (requires permission to manage features)
		[ClothXPermissionAuthorize("Feature")]
		public IActionResult CreateFeature(Feature model)
		{
			try
			{
				// Check if creating or editing, and call the appropriate utility method
				if (model.Id == 0)
				{
					FeaturesUtility.Instance.Addfeature(model, User.Identity.Name);
				}
				else
				{
					FeaturesUtility.Instance.EditFeature(model);
				}

				// Redirect to the details page of the associated feature group
				return RedirectToAction(nameof(Details), new { id = model.FeatureGroupId });
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to the server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to delete a feature (requires permission to manage features)
		[ClothXPermissionAuthorize("Feature")]
		public async Task<IActionResult> DeleteFeature(int? id)
		{
			try
			{
				// Check if the ID is null, then call the utility method to delete the feature
				if (id == null)
				{
					return NotFound();
				}

				Feature? feature = await FeaturesUtility.Instance.DeleteFeature(id);

				// Redirect to the details page of the associated feature group
				return RedirectToAction(nameof(Details), new { id = feature.FeatureGroupId });
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
