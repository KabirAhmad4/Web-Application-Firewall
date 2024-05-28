using Microsoft.AspNetCore.Authorization;

namespace ClothX.CustomAttributes
{
    internal class ClothXPermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public ClothXPermissionRequirement(string permission) { Permission = permission; }
    }
}