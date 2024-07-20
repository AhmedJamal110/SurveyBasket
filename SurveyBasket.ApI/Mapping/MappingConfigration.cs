using SurveyBasket.ApI.Contracts.Question;

namespace SurveyBasket.ApI.Mapping
{
    public class MappingConfigration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<QuestionRequest, Question>()
                .Map(dis => dis.Answers, src => src.Answers.Select(answer => new Answer { Content = answer }));
          
        
        
        }
    }
}
