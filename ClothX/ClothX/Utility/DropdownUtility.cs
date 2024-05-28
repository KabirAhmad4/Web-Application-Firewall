using ClothX.Constants;
using ClothX.DbModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClothX.Utility
{
	// Utility class for generating dropdowns
	public class DropdownUtility
	{
		// Singleton instance of the class
		private static DropdownUtility _instance;
		// Property to access the singleton instance
		public static DropdownUtility Instance
		{
			get
			{
				if (_instance == null)
					_instance = new DropdownUtility();
				return _instance;
			}
		}

		private DropdownUtility() { }

		// Generate a dropdown list for tailor project categories
		public List<SelectListItem> TailorProjectCategoryDropdown(bool withPrice = false)
		{
			
			ClothXDbContext db = new ClothXDbContext();

			// Retrieve a list of active product categories
			var lst = db.ProductCategories
				.Where(x => x.IsActive == true)
				.Select(a => new SelectListItem()
				{
					Text = a.Name + (withPrice == true ? " - " + a.Price + "Rs" : ""),
					Value = a.Id + ""
				})
				.ToList();

			return lst;
		}
	}
}