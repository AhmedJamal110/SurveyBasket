namespace SurveyBasket.ApI.Entities
{
    public sealed class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool Active { get; set; } = true;

        public int QuestionId { get; set; }
        public Question Question { get; set; } = default!;

    }
}
