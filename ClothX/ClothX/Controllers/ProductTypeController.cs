using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClothX.DbModels;
using ClothX.CustomAttributes;
using ClothX.Services;

namespace ClothX.Controllers
{
	public class ProductTypeController : Controller
	{
		// Action to display a list of product types (requires permission)
		[ClothXPermissionAuthorize("ProductTypes")]
		public async Task<IActionResult> Index()
		{
			try
			{
				// Initialize the database context and retrieve the list of product types
				ClothXDbContext db = new ClothXDbContext();
				var productTypes = await db.ProductCategories.ToListAsync();
				return View(productTypes);
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to a server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to display the add/edit product type partial view (requires permission)
		[HttpGet]
		[ClothXPermissionAuthorize("ProductTypes")]
		public IActionResult AddEdit(int Id = 0)
		{
			try
			{
				// Load the product type information for editing or create a new one
				ViewBag.Id = Id;
				ClothXDbContext db = new ClothXDbContext();
				if (Id != 0)
				{
					var productCategory = db.ProductCategories.Where(x => x.Id == Id).FirstOrDefault();
					return PartialView("CreateEditPartialView", productCategory);
				}
				return PartialView("CreateEditPartialView");
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to a server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to handle the creation or editing of a product type (requires permission)
		[HttpPost]
		[ClothXPermissionAuthorize("ProductTypes")]
		public async Task<IActionResult> Create(ProductCategory model)
		{
			try
			{
				// Save the new product type or update an existing one
				ClothXDbContext db = new ClothXDbContext();
				if (model.Id == 0)
				{
					model.AddedOn = DateTime.Now;
					model.AddedBy = User.Identity.Name;
					model.UpdatedOn = DateTime.Now;
					model.IsActive = true;

					db.ProductCategories.Add(model);
					await db.SaveChangesAsync();
				}
				else
				{
					var existing = db.ProductCategories.Where(x => x.Id == model.Id).FirstOrDefault();
					existing.Name = model.Name;
					existing.Price = model.Price;
					existing.UpdatedOn = DateTime.Now;

					db.ProductCategories.Update(existing);
					db.SaveChanges();
				}

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to a server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}

		// Action to activate or deactivate a product type (requires permission)
		[ClothXPermissionAuthorize("ProductTypes")]
		public async Task<IActionResult> Delete(int? id)
		{
			try
			{
				// Toggle the active status of the product type
				if (id == null)
				{
					return NotFound();
				}
				ClothXDbContext db = new ClothXDbContext();
				var productCategory = await db.ProductCategories
					.FirstOrDefaultAsync(m => m.Id == id);
				if (productCategory == null)
				{
					return NotFound();
				}

				productCategory.IsActive = !productCategory.IsActive;
				productCategory.UpdatedOn = DateTime.Now;
				db.ProductCategories.Update(productCategory);
				db.SaveChanges();

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				// Log any exceptions and redirect to a server error page
				ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
				return RedirectToAction("ServerError", "Error");
			}
		}
	}
}
