using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Traka.FobService.Model;

namespace Traka.FobService.Repositories
{
    /// <summary>
    /// Base repository that holds data in memory and mimics an async repository
    /// </summary>
    /// <typeparam name="T">Data Type</typeparam>
    public class BaseRepository<T>
        where T : Entity
    {
        protected readonly IList<T> _data;

        public BaseRepository(IList<T> data)
        {
            _data = data;
        }

        public Task<IEnumerable<T>> GetAll()
        {
            return Task.FromResult(_data.AsEnumerable());
        }

        public Task<T> GetByIdAsync(int id)
        {
            return Task.FromResult(_data.FirstOrDefault(e => e.Id == id));
        }

        public Task Add(T value)
        {
            _data.Add(value);
            return Task.CompletedTask;
        }

        public Task Update(int id, T value)
        {
            var entity = _data.FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                throw new KeyNotFoundException();
            }

            // Hacky update the entity by setting the id and removing the old one
            value.Id = entity.Id;

            _data.Remove(entity);
            _data.Add(value);
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var entity = _data.FirstOrDefault(e => e.Id == id);

            if (entity == null)
            {
                throw new KeyNotFoundException();
            }

            _data.Remove(entity);
            return Task.CompletedTask;
        }
    }
}
