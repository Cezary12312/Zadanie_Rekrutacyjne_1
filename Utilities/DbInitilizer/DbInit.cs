using Microsoft.EntityFrameworkCore;
using Zadanie_Rekrutacyjne_1.Data.DataAccess;
using Zadanie_Rekrutacyjne_1.Models;

namespace Zadanie_Rekrutacyjne_1.Utilities.DbInitilizer
{
    public class DbInit : IDbInit
        {
            private readonly UserDbContext context;

            public DbInit(UserDbContext context)
            {
                this.context = context;
            }

            public void Initialize()
            {
                try
                {
                    if (context.Database.GetPendingMigrations().Count() > 0)
                        context.Database.Migrate();
                }
                catch (Exception)
                {
                    throw;
                }
                if (context.Genders.Count() == 0)
                {
                    Gender man = new Gender()
                    {
                        GenderName = "Mężczyzna"
                    };
                    context.Genders.Add(man);
                    Gender woman = new Gender()
                    {
                        GenderName = "Kobieta"
                    };
                    context.Genders.Add(woman);
                    context.SaveChanges();
                }
            }
        }
    }
