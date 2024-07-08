using System.Security.Cryptography;

namespace SurveyBasket.ApI.Contracts.Polls
{
    public record PollResponse
    (
        int Id,
        string Title,
        string Summary,
        bool IsPublished,
        DateOnly StartsAt,
        DateOnly EndsAt
     );
}
