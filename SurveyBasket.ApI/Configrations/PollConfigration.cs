
namespace SurveyBasket.ApI.Configrations
{
    public class PollConfigration : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder.HasIndex(P => P.Title).IsUnique();
            builder.Property(P => P.Title).HasMaxLength(1500);
        }
    }
}
