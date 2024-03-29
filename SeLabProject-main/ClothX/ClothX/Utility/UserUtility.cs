using ClothX.Constants;
using ClothX.DbModels;
using System.Security.Claims;

namespace ClothX.Utility
{
	public class UserUtility
	{
		private static UserUtility _instance;

		public static UserUtility Instance
		{
			get
			{
				if (_instance == null)
					_instance = new UserUtility();
				return _instance;
			}
		}

		private UserUtility() { }

		
		// Gets the layout path based on the user's roles.
		public string GetLayout(ClaimsPrincipal? User)
		{
			string layout = "~/Views/Shared/_Layout.cshtml";
			//string dashboardLayout = "~/Views/Shared/_LayoutDashboard.cshtml";

			// Uncomment the following code if different layouts are used for specific roles
			//if (User == null)
			//{
			//    return layout;
			//}
			//if (User.IsInRole(RoleType.Tailor.ToString()))
			//{
			//    return layout;
			//}

			return layout;
		}

		
		// Checks if the user has the specified authority.
		public bool HasAuthority(string permission, ClaimsPrincipal? User)
		{
			if (User != null)
			{
				if (User.IsInRole(RoleType.Tailor.ToString()))
				{
					return true;
				}
			}
			return false;
		}

		
		// Gets the user's profile ID based on the username.
		public int getUserProfileId(string username)
		{
			ClothXDbContext db = new ClothXDbContext();
			var dbUser = db.AspNetUsers.Where(x => x.UserName == username).FirstOrDefault();
			return dbUser.UserProfiles.FirstOrDefault().Id;
		}
	}
}
