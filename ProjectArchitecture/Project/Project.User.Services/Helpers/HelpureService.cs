using Microsoft.AspNetCore.Identity;
using Project.Comman.Idenitity;
using System.Security.Claims;

namespace Project.Services.Helpers
{
    public class HelpureService : IHelpureService
    {
        #region fields
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region ctor
        public HelpureService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        #endregion

        #region GetUser
        public async Task<string> GetUserAsync(ClaimsPrincipal user)
        {
            var userData = await _userManager.GetUserAsync(user);

            if (userData == null)
                return string.Empty;

            return userData.Id;
        }
        #endregion
    }
}
