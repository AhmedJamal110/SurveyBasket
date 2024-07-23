namespace SurveyBasket.ApI.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;

        public int PollId { get; set; }
        public Poll Poll { get; set; } = default!;

        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = default!;

        public ICollection<VoteAnswer> VoteAnswers { get; set; } = [];
    }
}
