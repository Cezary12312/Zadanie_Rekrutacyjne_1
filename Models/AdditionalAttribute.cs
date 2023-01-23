using System.ComponentModel.DataAnnotations;

namespace Zadanie_Rekrutacyjne_1.Models
{
    public class AdditionalAttribute
    {
        [Key]
        public int AttributeId { get; set; }
        public string? AttributeName { get; set; }
        public string? AttributeValue { get; set; }
        public int UserId { get; set; }
    }
}
