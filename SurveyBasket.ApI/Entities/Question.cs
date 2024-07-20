namespace SurveyBasket.ApI.Entities
{
    public sealed class Question : BaseEntity
    {
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;
        // soft delete
        public bool IsActive { get; set; } = true;

        public int PollId { get; set; }
        public Poll Poll { get; set; } = default!;
        public ICollection<Answer> Answers { get; set; } = []; 


    }
}
