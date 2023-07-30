using KitsLib.Base.Models.Interfaces;

namespace KitsLib.Base.Repositories.Interfaces
{
    public interface IKitRepository : IRepository<IKit>
    {
        Task<IKit> FindByNameAsync(string kitName);
    }
}
