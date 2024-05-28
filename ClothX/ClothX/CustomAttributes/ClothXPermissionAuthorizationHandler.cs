using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ClothX.DbModels;

namespace ClothX.CustomAttributes
{
    // This class contains logic for determining whether MinimumAgeRequirements in authorization
    // policies are satisfied or not
    internal class ClothXPermissionAuthorizationHandler : AuthorizationHandler<ClothXPermissionRequirement>
    {
        private readonly ILogger<ClothXPermissionAuthorizationHandler> _logger;

        public ClothXPermissionAuthorizationHandler(ILogger<ClothXPermissionAuthorizationHandler> logger)
        {
            _logger = logger;
        }

        // Check whether a given MinimumAgeRequirement is satisfied or not for a particular context
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClothXPermissionRequirement requirement)
        {
            // Log as a warning so that it's very clear in sample output which authorization policies 
            // (and requirements/handlers) are in use
            _logger.LogWarning("Evaluating authorization requirement for permission = {Permission}", requirement.Permission);


            //Check the user Permissions
            bool flag = false;
            ClothXDbContext db = new ClothXDbContext();
            var userId = context.User.Identity.Name;

            var dbUser = db.AspNetUsers.Where(a => a.UserName.Equals(userId)).FirstOrDefault();


            /// Use this for Role Authorization
            if (dbUser != null)
            {
                var roles = dbUser.Roles;
                foreach (var role in roles)
                {
                    var permissions = role.Preveliges.ToList();
                    foreach (var p in permissions)
                    {
                        var pername = db.Preveliges.Find(p.Id);
                        if (pername.Name == requirement.Permission)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }

            // If the user meets the permission criterion, mark the authorization requirement succeeded
            if (flag)
            {
                _logger.LogInformation("Permission authorization requirement {permission} satisfied", requirement.Permission);
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Current user's claim  does not satisfy permission authorization requirement {permission}",

                    requirement.Permission);
            }

            return Task.CompletedTask;
        }
    }
}
