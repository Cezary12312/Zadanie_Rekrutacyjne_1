using Zadanie_Rekrutacyjne_1.Models;

namespace Zadanie_Rekrutacyjne_1.Data.Repository
{
    public interface IGenderRepository : IRepository<Gender>
    {
        void Update(Gender gender);
    }
}
