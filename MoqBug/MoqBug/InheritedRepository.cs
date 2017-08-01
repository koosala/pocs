using Eurofins.Common.DataConcerns;
using Eurofins.Common.DataConcerns.EF;

namespace MoqBug
{
    public class InheritedRepository : Repository<MyEntity>
    {
        public InheritedRepository(IUnitOfWork unitOfWork, EurofinsDbContext dbContext) : base(unitOfWork, dbContext)
        {
        }

        public virtual string GetString()
        {
            return "Hello from Inherited Repository";
        }
    }
}
