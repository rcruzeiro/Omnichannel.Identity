using Core.Framework.Repository;
using Omnichannel.Identity.Platform.Domain;

namespace Omnichannel.Identity.Platform.Infrastructure.Database.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWorkAsync unitOfWork)
            : base(unitOfWork)
        { }
    }
}
