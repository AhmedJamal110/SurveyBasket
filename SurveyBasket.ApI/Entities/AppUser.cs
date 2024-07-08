
namespace SurveyBasket.ApI.Entities
{
    public sealed class AppUser : IdentityUser
    {

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
    
        
    }
}
