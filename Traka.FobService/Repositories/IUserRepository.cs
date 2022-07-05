using System.Collections.Generic;
using System.Threading.Tasks;
using Traka.FobService.Model;

namespace Traka.FobService.Repositories
{
    /// <summary>
    /// User repository access with async methods
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Return all <see cref="User"/> from the repository
        /// </summary>
        Task<IEnumerable<User>> GetAll();

        /// <summary>
        /// Returns a single user 
        /// </summary>
        /// <param name="id">Id of the user to return</param>
        Task<User> GetByIdAsync(int id);

        /// <summary>
        /// Adds a user to the repository
        /// </summary>
        /// <param name="user">User to add</param>
        Task Add(User user);

        /// <summary>
        /// Updates an existing user in the repository
        /// </summary>
        /// <param name="id">Id of the user to update</param>
        /// <param name="user">User details to update</param>
        /// <returns></returns>
        Task Update(int id, User user);

        /// <summary>
        /// Delete a user from the repository
        /// </summary>
        /// <param name="id">Id of the user to delete</param>
        Task Delete(int id);
    }
}
