using Zadanie_Rekrutacyjne_1.Utilities;

namespace Zadanie_Rekrutacyjne_1.Models
{
    public class ReportElement
    {
        public MrMsStatus Status { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
    }
}
