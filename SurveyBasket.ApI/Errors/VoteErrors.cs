namespace SurveyBasket.ApI.Errors
{
    public class VoteErrors
    {
        public static readonly Error VoteDublicated =
            new("Vote.Dublicated", "this user had voted before ");
    }
}
