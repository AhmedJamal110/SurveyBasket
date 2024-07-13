namespace SurveyBasket.ApI.Contracts.Polls
{
    public record PollRequest
    (
        string Title,
        string Summary,
        DateOnly StartsAt,
        DateOnly EndsAt

      );
}
