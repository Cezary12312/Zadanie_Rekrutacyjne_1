using Microsoft.EntityFrameworkCore;
using Zadanie_Rekrutacyjne_1.Models;

namespace Zadanie_Rekrutacyjne_1.Data.DataAccess
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<AdditionalAttribute> AdditionalAttributes { get; set; }
        public DbSet<Gender> Genders { get; set; }
    }
}
