using ClothX.Constants;
using ClothX.DbModels;
using ClothX.Services;
using ClothX.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ClothX.Utility
{
	// Utility class for managing tailor projects
	public class TailorProjectsUtility
	{
		// Singleton instance of the class
		private static TailorProjectsUtility _instance;
		private static ClothXDbContext db;

		// Property to access the singleton instance
		public static TailorProjectsUtility Instance
		{
			get
			{
				if (_instance == null)
					_instance = new TailorProjectsUtility();
				if (db == null)
					db = new ClothXDbContext();
				return _instance;
			}
		}

		// Private constructor to enforce singleton pattern
		private TailorProjectsUtility() { }

		// Get a list of active tailor projects
		public async Task<List<TailorProject>> getActiveTailorProjects()
		{
			var projects = await db.TailorProjects.Where(x => x.IsActive == true).ToListAsync();
			return projects;
		}

		// Get a list of all tailor projects
		public async Task<List<TailorProject>> getTailorProjects()
		{
			var projects = await db.TailorProjects.ToListAsync();
			return projects;
		}

		// Map a TailorProject object to a TailorProjectViewModel object
		private TailorProjectViewModel getViewModel(TailorProject project)
		{
			TailorProjectViewModel model = new TailorProjectViewModel();
			model.Id = project.Id;
			model.Title = project.Title;
			model.Description = project.Description;
			model.ImagePath = project.ImagePath;
			model.AddedBy = project.AddedBy;
			model.AddedOn = project.AddedOn;
			model.UpdatedOn = project.UpdatedOn;
			model.ProductCategoryId = project.ProductCategoryId;
			model.CategoryName = project.ProductCategory.Name;
			model.IsActive = project.IsActive;
			model.TailorProjectImages = project.TailorProjectImages;

			return model;
		}

		// Get a tailor project by ID
		public async Task<TailorProjectViewModel?> getProject(int id)
		{
			var dbProject = await db.TailorProjects.Where(x => x.Id == id).FirstOrDefaultAsync();
			if (dbProject == null)
			{
				return null;
			}
			return getViewModel(dbProject);
		}

		// Get a tailor project by ID (database model)
		public async Task<TailorProject?> getProjectDbModel(int id)
		{
			var dbProject = await db.TailorProjects.Where(x => x.Id == id).FirstOrDefaultAsync();
			if (dbProject == null)
			{
				return null;
			}
			return dbProject;
		}

		// Activate or deactivate a tailor project
		public async Task ActiveInActiveProject(int projectId)
		{
			var proj = await db.TailorProjects.Where(x => x.Id == projectId).FirstOrDefaultAsync();
			proj.IsActive = !proj.IsActive;
			proj.UpdatedOn = DateTime.Now;
			db.TailorProjects.Update(proj);
			await db.SaveChangesAsync();
		}

		// Map a TailorProjectViewModel object to a TailorProject object
		public TailorProject GetDbModel(TailorProjectViewModel proj)
		{
			TailorProject model = new TailorProject();
			model.Title = proj.Title;
			model.Description = proj.Description;
			model.AddedOn = proj.AddedOn;
			model.AddedBy = proj.AddedBy;
			model.UpdatedOn = proj.UpdatedOn;
			model.IsActive = proj.IsActive;
			model.ProductCategoryId = proj.ProductCategoryId;
			return model;
		}

		// Add a new tailor project to the database
		public async Task AddTailorProject(TailorProjectViewModel proj, string username, IWebHostEnvironment webHost)
		{
			var model = GetDbModel(proj);
			// Set additional properties for the new TailorProject
			model.IsActive = true;
			model.AddedOn = DateTime.Now;
			model.UpdatedOn = DateTime.Now;
			model.AddedBy = username;

			// List of file paths for uploading images
			List<FilePathEnum> filepath = new()
				{
					FilePathEnum.Images,
					FilePathEnum.ProjectImages
				};

			// Upload the main project image and update the ImagePath property
			model.ImagePath = await UploadFileService.Instance.UploadFile(proj.image, filepath, webHost);

			// Add the new TailorProject to the TailorProjects table in the database
			await db.TailorProjects.AddAsync(model);
			await db.SaveChangesAsync();


			foreach (var s in proj.projectImages)
			{

				TailorProjectImage img = new TailorProjectImage();

				// Set the project ID and upload the image
				img.ProjectId = model.Id;
				img.ImagePath = await UploadFileService.Instance.UploadFile(s, filepath, webHost);


				await db.TailorProjectImages.AddAsync(img);
				await db.SaveChangesAsync();
			}

		}
	
	

		// Edit an existing tailor project in the database
		public async Task EditTailorProject(TailorProjectViewModel proj, string username, IWebHostEnvironment webHost)
		{
			// Get the tailor project from the database based on the provided ID
			var dbModel = await db.TailorProjects.Where(x => x.Id == proj.Id).FirstOrDefaultAsync();

			// Update the properties of the tailor project with the values from the view model
			dbModel.Title = proj.Title;
			dbModel.Description = proj.Description;
			dbModel.ProductCategoryId = proj.ProductCategoryId;
			dbModel.UpdatedOn = DateTime.Now;

			// List of file paths for uploading images
			List<FilePathEnum> filepath = new()
				{
					FilePathEnum.Images,
					FilePathEnum.ProjectImages
				};

			if (proj.ImagePath != null)
			{
				// Upload the new image and update the ImagePath property
				dbModel.ImagePath = await UploadFileService.Instance.UploadFile(proj.image, filepath, webHost);
			}

			// Update the tailor project in the database
			db.TailorProjects.Update(dbModel);
			await db.SaveChangesAsync();


			if (proj.projectImages != null)
			{
				foreach (var s in proj.projectImages)
				{

					TailorProjectImage img = new TailorProjectImage();

					// Set the project ID and upload the image
					img.ProjectId = dbModel.Id;
					img.ImagePath = await UploadFileService.Instance.UploadFile(s, filepath, webHost);
					await db.TailorProjectImages.AddAsync(img);
					await db.SaveChangesAsync();
				}
			}

		}
	}

}

