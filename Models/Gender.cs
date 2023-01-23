using System.ComponentModel.DataAnnotations;

namespace Zadanie_Rekrutacyjne_1.Models
{
    public class Gender
    {
        [Key]
        public int GenderId { get; set; }
        public string? GenderName { get; set; }
    }
}
