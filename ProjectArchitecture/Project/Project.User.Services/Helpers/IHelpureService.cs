using System.Security.Claims;

namespace Project.Services.Helpers
{
    public interface IHelpureService
    {
        public Task<string> GetUserAsync(ClaimsPrincipal user);
    }
}
