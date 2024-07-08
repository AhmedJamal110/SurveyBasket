
namespace SurveyBasket.ApI.Configrations
{
    public class AppUserConfigration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(U => U.FirstName).HasMaxLength(100);
            builder.Property(U => U.LastName).HasMaxLength(100);

        }
    }
}
