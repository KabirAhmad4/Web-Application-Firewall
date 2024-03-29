using ClothX.Constants;
using ClothX.DbModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClothX.Utility
{
	// Utility class for handling lookups and dropdowns
	public class LookupUtility
	{
		// Singleton instance of the class
		private static LookupUtility _instance;

		// Property to access the singleton instance
		public static LookupUtility Instance
		{
			get
			{
				if (_instance == null)
					_instance = new LookupUtility();
				return _instance;
			}
		}

	
		private LookupUtility() { }

		// Get a SelectList for a specific lookup category
		public List<SelectListItem> getSelectList(LookupCategory category)
		{
			ClothXDbContext db = new ClothXDbContext();
			var lst = db.Lookups
				.Where(x => x.Category.ToUpper() == category.ToString().ToUpper())
				.Select(a => new SelectListItem()
				{
					Text = a.Value,
					Value = a.Id + ""
				})
				.ToList();
			return lst;
		}

		// Get the value of a lookup item by ID
		public string getValue(int id)
		{
			if (id == 0)
			{
				return "";
			}
			ClothXDbContext db = new ClothXDbContext();
			string value = db.Lookups
				.Where(x => x.Id == id)
				.FirstOrDefault()
				.Value;
			return value;
		}

		// Get the ID of a lookup item by value
		public int getId(string value)
		{
			ClothXDbContext db = new ClothXDbContext();
			int id = db.Lookups
				.Where(x => x.Value == value)
				.FirstOrDefault()
				.Id;
			return id;
		}

		// Get the ID of the first lookup item in a category
		public int GetFirstObjectId(LookupCategory category)
		{
			ClothXDbContext db = new ClothXDbContext();
			int id = db.Lookups
				.Where(x => x.Category.ToUpper() == category.ToString().ToUpper())
				.FirstOrDefault()
				.Id;
			return id;
		}
	}
}
