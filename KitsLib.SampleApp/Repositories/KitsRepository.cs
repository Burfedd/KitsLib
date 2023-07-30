using KitsLib.Base.Models.Interfaces;
using KitsLib.Base.Repositories.Interfaces;

namespace KitsLib.SampleApp.Repositories
{
    public class KitsRepository : IKitRepository
    {
        private static List<IKit> _storage = new();

        public Task<IKit> FindByNameAsync(string kitName)
        {
            try
            {
                IKit result = _storage.SingleOrDefault(kit => kit.Name == kitName);
                return Task.FromResult(result);
            }
            catch
            {
                return null;
            }
        }

        public Task<IEnumerable<IKit>> GetAllAsync()
        {
            return Task.FromResult(_storage as IEnumerable<IKit>);
        }

        public Task<IKit> InsertAsync(IKit entity)
        {
            _storage.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<IEnumerable<IKit>> InsertRangeAsync(IEnumerable<IKit> entities)
        {
            _storage.AddRange(entities);
            return Task.FromResult(_storage as IEnumerable<IKit>);
        }
    }
}
