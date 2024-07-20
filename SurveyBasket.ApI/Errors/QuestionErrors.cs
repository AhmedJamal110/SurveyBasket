namespace SurveyBasket.ApI.Errors
{
    public class QuestionErrors
    {
        public static readonly Error QuestionNotFound
            = new("Question.not found", "no question was found was the same Id");

        public static readonly Error DuplicatedQuestionContent
            = new("Question.Duplicated", "Another Question was found with the same Content");
    }
}
