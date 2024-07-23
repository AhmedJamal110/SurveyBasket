using SurveyBasket.ApI.Contracts.Polls;
using SurveyBasket.ApI.Models;

namespace SurveyBasket.ApI.Services
{
    public interface IPollServices
    {
        Task<IReadOnlyList<Poll>> GetALlPolls(CancellationToken cancellationToken= default);
        Task<TResult<PollResponse>> GetPollById(int id , CancellationToken cancellationToken = default);

        Task<IEnumerable<PollResponse>> GetCurrentAsync(CancellationToken cancellationToken);
        Task<TResult<PollResponse>> AddPoll(PollRequest pollRequest , CancellationToken cancellationToken = default);
        Task<Result> UpdatePoll(int id , PollRequest pollRequest , CancellationToken cancellationToken = default);
        Task<bool> DeletePoll(int id , CancellationToken cancellationToken = default);

        Task<bool> ToggelStatue(int id, CancellationToken cancellationToken = default);


    }
}
