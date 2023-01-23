using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Zadanie_Rekrutacyjne_1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string? Name { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string? LastName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public int GenderForeignKey { get; set; }
        [Required]
        [ForeignKey("GenderForeignKey")]
        public Gender? Gender { get; set; }
        public List<AdditionalAttribute> AdditionalAttributes { get; set; }
    }
}
