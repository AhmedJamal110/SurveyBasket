using System.Security.Claims;

namespace SurveyBasket.ApI.DataDbContext
{
    public class ApplicationDbContext :IdentityDbContext<AppUser>  
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option , IHttpContextAccessor httpContextAccessor)
            :base(option)
        {
            _httpContextAccessor = httpContextAccessor;
        }

                    // override modelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var cascadeFks = modelBuilder.Model
                                                            .GetEntityTypes()
                                                            .SelectMany(t => t.GetForeignKeys())
                                                            .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade);


            foreach (var fk in cascadeFks)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }


                    /// ovride saveChanges
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
               
            
            var entities =  ChangeTracker.Entries<BaseEntity>();
            foreach (var entity in entities)
            {
                var currentUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if( entity.State == EntityState.Added)
                {
                    entity.Property(x => x.CreatedById).CurrentValue = currentUser; 
                }
                else if(entity.State == EntityState.Modified)
                {
                    entity.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;

                    entity.Property(x => x.UpdatedById).CurrentValue = currentUser ;
                }
            
            
            } 

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Poll> Polls { get; set; }
        public DbSet<Question>  Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<VoteAnswer> VoteAnswers { get; set; }
    }
}
