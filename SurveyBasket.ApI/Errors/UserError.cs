namespace SurveyBasket.ApI.Errors
{
    public static class UserError
    {
        public static readonly Error InvalidCredential = 
            new("User.Invalid Credential ", "Invalid email or password"); 
 

    }
}
