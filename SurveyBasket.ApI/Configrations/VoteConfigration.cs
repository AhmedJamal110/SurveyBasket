
namespace SurveyBasket.ApI.Configrations
{
    public class VoteConfigration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasIndex(x => new { x.PollId, x.UserId }).IsUnique();
                
        }
    }
}
