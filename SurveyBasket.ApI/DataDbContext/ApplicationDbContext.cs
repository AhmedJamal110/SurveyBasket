
namespace SurveyBasket.ApI.DataDbContext
{
    public class ApplicationDbContext :IdentityDbContext<AppUser>  
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option):base(option)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Poll> Polls { get; set; }
    }
}
