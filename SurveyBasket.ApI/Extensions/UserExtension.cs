using System.Security.Claims;

namespace SurveyBasket.ApI.Extensions
{
    public static class UserExtension 
    {
        public static string? GetUserID(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }

}
