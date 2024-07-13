namespace SurveyBasket.ApI.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; } 


        public string CreatedById { get; set; } = string.Empty;
        public AppUser CreatedBy { get; set; } = default!;

        
        public string? UpdatedById { get; set; }
        public AppUser? UpdatedBy { get; set; } 

    }
}
