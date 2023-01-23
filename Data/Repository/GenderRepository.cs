using Zadanie_Rekrutacyjne_1.Data.DataAccess;
using Zadanie_Rekrutacyjne_1.Models;

namespace Zadanie_Rekrutacyjne_1.Data.Repository
{
    public class GenderRepository : Repository<Gender>, IGenderRepository
    {
        private UserDbContext context;
        public GenderRepository(UserDbContext context) : base(context) => this.context = context;

        public void Update(Gender gender)
        {
            var genderDb = context.Genders.FirstOrDefault(x => x.GenderId == gender.GenderId);
            if (genderDb != null)
            {
                genderDb.GenderName = gender.GenderName;
            }
        }
    }
}
