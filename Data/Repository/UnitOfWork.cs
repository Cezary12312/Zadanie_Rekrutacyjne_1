using Zadanie_Rekrutacyjne_1.Data.DataAccess;

namespace Zadanie_Rekrutacyjne_1.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private UserDbContext context;
        public IUserRepository UserRepository { get; private set; }
        public IGenderRepository GenderRepository { get; private set; }

        public IAttributeRepository AttributeRepository { get; private set; }

        public UnitOfWork(UserDbContext context)
        {
            this.context = context;
            UserRepository = new UserRepository(context);
            GenderRepository = new GenderRepository(context);
            AttributeRepository = new AttributeRepository(context);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
