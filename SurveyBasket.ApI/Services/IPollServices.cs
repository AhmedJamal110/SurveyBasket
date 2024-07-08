using SurveyBasket.ApI.Models;

namespace SurveyBasket.ApI.Services
{
    public interface IPollServices
    {
        Task<IReadOnlyList<Poll>> GetALlPolls(CancellationToken cancellationToken= default);
        Task<Poll?> GetPollById(int id , CancellationToken cancellationToken = default);
        Task<Poll> AddPoll(Poll poll , CancellationToken cancellationToken = default);
        Task<bool> UpdatePoll(int id , Poll poll , CancellationToken cancellationToken = default);
        Task<bool> DeletePoll(int id , CancellationToken cancellationToken = default);

        Task<bool> ToggelStatue(int id, CancellationToken cancellationToken = default);


    }
}
