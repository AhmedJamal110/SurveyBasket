namespace SurveyBasket.ApI.Contracts.Question
{
    public record QuestionRequest 
        (
            string Content,
            ICollection<string> Answers
        );  
       
}
