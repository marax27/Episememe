using System.Linq;
using System.Security.Claims;

namespace Episememe.Api.Utilities
{
    public static class ClaimsExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
            => user.Claims
                .SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)
                ?.Value ?? "";
    }
}
