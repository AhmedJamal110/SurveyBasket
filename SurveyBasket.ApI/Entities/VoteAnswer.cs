namespace SurveyBasket.ApI.Entities
{
    public class VoteAnswer
    {
        public int Id { get; set; }
        public int VoteId { get; set; }
        public Vote Vote { get; set; } = default!;

        public int AnswerId { get; set; }
        public Answer Answer { get; set; } = default!;

        public int QuestionId { get; set; }
        public Question Question { get; set; } = default!;
    }
}
