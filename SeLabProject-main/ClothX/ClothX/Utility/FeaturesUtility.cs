using ClothX.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ClothX.Utility
{
	// Utility class for managing features and feature groups
	public class FeaturesUtility
	{
		// Singleton instance of the class
		private static FeaturesUtility _instance;

		// Property to access the singleton instance
		public static FeaturesUtility Instance
		{
			get
			{
				if (_instance == null)
					_instance = new FeaturesUtility();
				return _instance;
			}
		}

		
		private FeaturesUtility() { }

		// Get all feature groups
		public List<FeatureGroup> GetFeatures()
		{
			ClothXDbContext db = new ClothXDbContext();
			var features = db.FeatureGroups.ToList();
			return features;
		}

		// Find a feature group by ID
		public FeatureGroup? FindFeatureGroup(int Id)
		{
			ClothXDbContext db = new ClothXDbContext();
			var feature = db.FeatureGroups.Find(Id);
			return feature;
		}

		// Edit a feature group
		public void EditFeatureGroup(FeatureGroup model)
		{
			ClothXDbContext db = new ClothXDbContext();
			var existing = db.FeatureGroups.Find(model.Id);
			existing.Name = model.Name;
			existing.UpdatedOn = DateTime.Now;
			db.FeatureGroups.Update(existing);
			db.SaveChanges();
		}

		// Delete a feature group
		public async Task DeleteFeatureGroup(int? id)
		{
			ClothXDbContext db = new ClothXDbContext();
			var featureGroup = await db.FeatureGroups
				.FirstOrDefaultAsync(m => m.Id == id);

			featureGroup.IsActive = !featureGroup.IsActive;
			featureGroup.UpdatedOn = DateTime.Now;
			db.FeatureGroups.Update(featureGroup);
			db.SaveChanges();
		}

		// Find a feature by ID
		public Feature? FindFeature(int id)
		{
			ClothXDbContext db = new ClothXDbContext();
			var feature = db.Features.Find(id);
			return feature;
		}

		// Edit a feature
		public void EditFeature(Feature model)
		{
			ClothXDbContext db = new ClothXDbContext();
			var existing = db.Features.Find(model.Id);
			existing.Name = model.Name;
			existing.Description = model.Description;
			existing.Price = model.Price;
			existing.UpdatedOn = DateTime.Now;
			db.Features.Update(existing);
			db.SaveChanges();
		}

		// Add a new feature
		public void Addfeature(Feature model, string username)
		{
			ClothXDbContext db = new ClothXDbContext();
			model.AddedOn = DateTime.Now;
			model.AddedBy = username;
			model.UpdatedOn = DateTime.Now;
			model.IsActive = true;
			db.Features.Add(model);
			db.SaveChanges();
		}

		// Delete a feature
		public async Task<Feature?> DeleteFeature(int? id)
		{
			ClothXDbContext db = new ClothXDbContext();
			var feature = await db.Features
				.FirstOrDefaultAsync(m => m.Id == id);

			feature.IsActive = !feature.IsActive;
			feature.UpdatedOn = DateTime.Now;
			db.Features.Update(feature);
			db.SaveChanges();
			return feature;
		}

		// Add a new feature group
		public void AddFeatureGroup(FeatureGroup model, string username)
		{
			ClothXDbContext db = new ClothXDbContext();
			model.AddedOn = DateTime.Now;
			model.AddedBy = username;
			model.UpdatedOn = DateTime.Now;
			model.IsActive = true;
			db.FeatureGroups.Add(model);
			db.SaveChanges();
		}

		// Get all active feature groups that have at least one active feature
		public List<FeatureGroup> getFeatureGroups()
		{
			ClothXDbContext db = new ClothXDbContext();
			var featureGroups = db.FeatureGroups
				.Where(x => x.IsActive == true)
				.ToList();

			featureGroups = featureGroups
				.Where(x => x.Features.Any(x => x.IsActive == true))
				.ToList();

			return featureGroups;
		}
	}
}
