using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Zadanie_Rekrutacyjne_1.Data.DataAccess;
using Zadanie_Rekrutacyjne_1.Models;

namespace Zadanie_Rekrutacyjne_1.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private UserDbContext context;
        private DbSet<User> set;
        public UserRepository(UserDbContext context) : base(context) => this.context = context;
        public void Update(User user)
        {
            for (int i = user.AdditionalAttributes.Count - 1; i >= 0; i--)
            {
                var item = user.AdditionalAttributes[i];
                if (item.AttributeValue == null || item.AttributeName == null)
                    user.AdditionalAttributes.Remove(item);
            }
            var userDb = context.Users.FirstOrDefault(x => x.Id == user.Id);
            if (userDb != null)
            {
                userDb.Name = user.Name;
                userDb.LastName = user.LastName;
                userDb.DateOfBirth = user.DateOfBirth;
                userDb.Gender = user.Gender;
                userDb.AdditionalAttributes = new List<AdditionalAttribute>() { user.AdditionalAttributes[user.AdditionalAttributes.Count - 1] };
            }
        }
    }
}
