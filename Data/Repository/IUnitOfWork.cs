namespace Zadanie_Rekrutacyjne_1.Data.Repository
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IGenderRepository GenderRepository { get; }
        IAttributeRepository AttributeRepository { get; }
        void Save();
    }
}
