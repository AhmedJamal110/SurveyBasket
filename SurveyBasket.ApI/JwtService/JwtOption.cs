using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.ApI.JwtService
{
    public class JwtOption
    {
        public static string SectionName { get; set; } = "Token";
        [Required]
        public string key { get; set; } = string.Empty;
        [Required]

        public string ValidIssuer { get; set; } = string.Empty;
        [Required]

        public string ValidAudiance { get; set; } = string.Empty;
        [Required]
        [Range(1 , int.MaxValue)]
        public int  ExpireMinutes  { get; set; } 



       }
}
