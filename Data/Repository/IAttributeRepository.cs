using Zadanie_Rekrutacyjne_1.Models;

namespace Zadanie_Rekrutacyjne_1.Data.Repository
{
    public interface IAttributeRepository : IRepository<AdditionalAttribute>
    {
        void Update(AdditionalAttribute attribute);
    }
}
