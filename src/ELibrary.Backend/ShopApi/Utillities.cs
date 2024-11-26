using System.Security.Claims;

namespace ShopApi
{
    public static class Utilities
    {
        public static string GetUserId(ClaimsPrincipal principal)
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new InvalidDataException("Can not find user id!");
            }

            return userId;
        }
    }
}
