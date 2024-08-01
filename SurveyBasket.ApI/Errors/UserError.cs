namespace SurveyBasket.ApI.Errors
{
    public static class UserError
    {
        public static readonly Error InvalidCredential = 
            new("User.Invalid Credential ", "Invalid email or password");


        public static readonly Error ConfirmEmail =
      new("User.EmailNotConfairm ", "Email not Comfairmed");


        public static readonly Error InvalidCodeOrToken =
    new("user.Invalid Code OR Token", "invlaid code ");


        public static readonly Error DuplicatedEmailConfairm =
            new("user.Deplicated Email Coanfiemed", "email had already confairmed before");
    
    }
}
