using Eurofins.Common.DataConcerns;
using Eurofins.Common.DataConcerns.EF;

namespace MoqBug
{
    public class StandAloneRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EurofinsDbContext _dbContext;

        public StandAloneRepository(IUnitOfWork unitOfWork, EurofinsDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }
        public virtual string GetString()
        {
            return "Hello from Standalone Repository";
        }
    }
}
