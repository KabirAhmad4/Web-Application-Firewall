using ClothX.CustomAttributes;
using ClothX.DbModels;
using ClothX.Services;
using ClothX.Utility;
using ClothX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ClothX.Controllers
{
	public class ProjectController : Controller
	{
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProjectController(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		// Action to display a list of active tailor projects
		public async Task<IActionResult> Index()
		{
			List<TailorProject> tailorProjects = new List<TailorProject>();
			try
			{
				tailorProjects = await TailorProjectsUtility.Instance.getActiveTailorProjects();
			}
			catch (Exception ex)
			{
				// Log any exceptions and continue
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return View(tailorProjects);
		}

		// Action to manage tailor projects (requires permission)
		[ClothXPermissionAuthorize("TailorProjects")]
		public async Task<IActionResult> Manage()
		{
			List<TailorProject> tailorProjects = new List<TailorProject>();
			try
			{
				tailorProjects = await TailorProjectsUtility.Instance.getTailorProjects();
			}
			catch (Exception ex)
			{
				// Log any exceptions and continue
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}
			return View(tailorProjects);
		}

		// Action to add or edit a tailor project (requires permission)
		[HttpGet]
		[ClothXPermissionAuthorize("TailorProjects")]
		public async Task<IActionResult> AddEdit(int Project = 0)
		{
			try
			{
				// Load dropdown data and prepare the view
				ViewBag.ProjectCategory = DropdownUtility.Instance.TailorProjectCategoryDropdown();
				ViewBag.ProjectId = Project;

				// If Project is 0, it is a new project, else retrieve the existing project
				if (Project == 0)
				{
					return View();
				}

				var viewModel = await TailorProjectsUtility.Instance.getProject(Project);
				return View(viewModel);
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to a server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to handle the form submission for adding or editing a tailor project (requires permission)
		[HttpPost]
		[ClothXPermissionAuthorize("TailorProjects")]
		public async Task<IActionResult> AddEdit(TailorProjectViewModel model, IFormCollection form)
		{
			try
			{
				// Add or edit the tailor project based on the provided model
				if (model.Id == 0)
				{
					await TailorProjectsUtility.Instance.AddTailorProject(model, User.Identity.Name, _webHostEnvironment);
				}
				else
				{
					await TailorProjectsUtility.Instance.EditTailorProject(model, User.Identity.Name, _webHostEnvironment);
				}

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to a server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to activate or deactivate a tailor project (requires permission)
		[ClothXPermissionAuthorize("TailorProjects")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				// Activate or deactivate the project based on the provided id
				await TailorProjectsUtility.Instance.ActiveInActiveProject(id);
			}
			catch (Exception ex)
			{
				// Log any exceptions and continue
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}

			return RedirectToAction("Index");
		}

		// Action to display details of a specific tailor project
		public async Task<IActionResult> Details(int Project)
		{
			try
			{
				// Retrieve the project details from the database
				var model = await TailorProjectsUtility.Instance.getProjectDbModel(Project);

				// If the model is null, the project does not exist
				if (model == null)
				{
					throw new Exception("Project with Id= " + Project + " does not exist.");
				}

				return View(model);
			}
			catch (Exception ex)
			{
				// Log any exceptions and continue
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
			}

			return RedirectToAction("Index");
		}
	}
}
