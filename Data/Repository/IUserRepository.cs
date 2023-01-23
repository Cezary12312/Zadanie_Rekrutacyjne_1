using Zadanie_Rekrutacyjne_1.Models;

namespace Zadanie_Rekrutacyjne_1.Data.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User user);
    }
}
