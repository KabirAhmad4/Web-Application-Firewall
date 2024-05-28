using ClothX.Constants;
using ClothX.CustomAttributes;
using ClothX.DbModels;
using ClothX.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothX.Controllers
{
    // User controller class for handling user-related actions
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment webHost;

        public UserController(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
        }

        // Action method for displaying a list of user profiles
        [ClothXPermissionAuthorize("Users")]
        public IActionResult Index()
        {
            try
            {
                ClothXDbContext db = new ClothXDbContext();
                // Retrieve all user profiles from the database
                var dbUsers = db.UserProfiles.ToList();
                return View(dbUsers);
            }
            catch (Exception ex)
            {
                // Log any exceptions and redirect to a server error page
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
                return RedirectToAction("ServerError", "Error");
            }
        }

        // Action method for approval status of a user
        [ClothXPermissionAuthorize("Users")]
        public IActionResult ApprovalStatusChange(int id)
        {
            try
            {
                ClothXDbContext db = new ClothXDbContext();
                // Retrieve a specific user profile by ID
                var dbUser = db.UserProfiles.Where(x => x.Id == id).FirstOrDefault();
                // Toggle the approval status
                dbUser.IsApproved = !dbUser.IsApproved;
                db.UserProfiles.Update(dbUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log any exceptions and redirect to a server error page
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
                return RedirectToAction("ServerError", "Error");
            }
        }

        // Action method for displaying the user's profile
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            try
            {
                ClothXDbContext db = new ClothXDbContext();
                // Retrieve the user profile associated with the currently logged-in user
                var dbUsers = await db.AspNetUsers.Where(x => x.UserName == User.Identity.Name).ToListAsync();
                var _user = dbUsers.FirstOrDefault()?.UserProfiles.FirstOrDefault();

                return View(_user);
            }
            catch (Exception ex)
            {
                // Log any exceptions and redirect to a server error page
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
                return RedirectToAction("ServerError", "Error");
            }
        }

        // Action method for updating the user's profile
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(IFormCollection form)
        {
            try
            {
                ClothXDbContext db = new ClothXDbContext();
                // Retrieve the user profile to be updated by ID
                int Id = int.Parse(form["Id"]);
                var _user = await db.UserProfiles.Where(x => x.Id == Id).FirstOrDefaultAsync();

                // Update user profile information based on form data
                _user.FirstName = form["FirstName"];
                _user.LastName = form["LastName"];
                _user.PhoneNumber = form["PhoneNumber"];
                _user.Address = form["Address"];

                // Upload a new profile image if provided in the form
                if (form.Files.Any(x => x.Name == "ProfileImage"))
                {
                    List<FilePathEnum> filepath = new()
                    {
                        FilePathEnum.Images,
                        FilePathEnum.UserImages
                    };
                    var file = form.Files.Where(x => x.Name == "ProfileImage").FirstOrDefault();
                    _user.ImagePath = await UploadFileService.Instance.UploadFile(file, filepath, webHost);
                }

                // Update the user profile in the database
                db.UserProfiles.Update(_user);
                await db.SaveChangesAsync();

                return RedirectToAction("Profile");
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
