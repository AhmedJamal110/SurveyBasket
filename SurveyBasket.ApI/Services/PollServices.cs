using SurveyBasket.ApI.DataDbContext;
using SurveyBasket.ApI.Models;

namespace SurveyBasket.ApI.Services
{
    public class PollServices : IPollServices
    {
        private readonly ApplicationDbContext _context;

        public PollServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Poll> AddPoll(Poll poll , CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(poll , cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return poll;
        }

        public async Task<bool> DeletePoll(int id , CancellationToken cancellationToken = default)
        {
            var poll = await _context.Polls.FindAsync(id , cancellationToken);
            if (poll is null)
                return false;

            _context.Polls.Remove(poll );
          await   _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IReadOnlyList<Poll>> GetALlPolls(CancellationToken cancellationToken = default)
        =>  await _context.Polls.AsNoTracking().ToListAsync(cancellationToken);


        public async Task<Poll?> GetPollById(int id , CancellationToken cancellationToken = default)
            => await _context.Polls.FindAsync(id , cancellationToken);

        public async Task<bool> ToggelStatue(int id, CancellationToken cancellationToken = default)
        {
           var pollInDb =  await _context.Polls.FindAsync(id, cancellationToken);
            if (pollInDb is null)
                return false;

            pollInDb.IsPublished = !pollInDb.IsPublished;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePoll(int id , Poll poll , CancellationToken cancellationToken = default)
        {
           var pollInDb =  await _context.Polls.FindAsync(id , cancellationToken);
            if (pollInDb is null)
                return false;
               
            pollInDb.Id = id;
            pollInDb.Title = poll.Title;
            pollInDb.Summary = poll.Summary;
            pollInDb.StartsAt = poll.StartsAt;
            pollInDb.EndsAt = poll.EndsAt;
            pollInDb.IsPublished = poll.IsPublished;

            
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
