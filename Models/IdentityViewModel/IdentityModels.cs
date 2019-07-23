using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LibraryProject.Models.IdentityViewModel
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("NameOfUser", this.NameOfUser));
            return userIdentity;
        }
        // My Extended Properties
        public string NameOfUser { get; set; }

        
    }
    public static class IdentityExtensions
    {
        public static string Get(this System.Security.Principal.IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("NameOfUser");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}