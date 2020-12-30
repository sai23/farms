using System.Security.Claims;

namespace API.Extentions
{
    public static class ClaimsPrincipleExtentions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           
        }
    }
}