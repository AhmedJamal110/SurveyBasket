namespace SurveyBasket.ApI.Errors
{
    public static class PollErrors
    {
        public static readonly Error PollNotFound
            = new("Poll.NotFound ", "no poll wans not found with this ID");

    }
}
