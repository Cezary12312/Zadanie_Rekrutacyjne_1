using Microsoft.EntityFrameworkCore;
using Zadanie_Rekrutacyjne_1.Data.DataAccess;
using Zadanie_Rekrutacyjne_1.Models;

namespace Zadanie_Rekrutacyjne_1.Data.Repository
{
    public class AttributeRepository : Repository<AdditionalAttribute>, IAttributeRepository
    {
        private UserDbContext context;
        private DbSet<AdditionalAttribute> set;
        public AttributeRepository(UserDbContext context) : base(context) => this.context = context;
        public void Update(AdditionalAttribute attribute)
        {
            var attributeDb = context.AdditionalAttributes.FirstOrDefault(x => x.AttributeName == attribute.AttributeName && x.AttributeValue == attribute.AttributeValue);
            if (attributeDb != null)
            {
                attributeDb.AttributeName = attribute.AttributeName;
                attributeDb.AttributeValue = attribute.AttributeValue;
            }
        }
    }
}
