using System.Collections.Generic;
using System.Threading.Tasks;
using Traka.FobService.Model;

namespace Traka.FobService.Repositories
{
    /// <summary>
    /// System repository access with async methods
    /// </summary>
    public interface ISystemRepository
    {
        /// <summary>
        /// Returns all <see cref="TrakaSystem"/> from the repository
        /// </summary>
        Task<IEnumerable<TrakaSystem>> GetAll();

        /// <summary>
        /// Assigns the fob to the user
        /// </summary>
        /// <param name="systemId">Id of the system where the fob belongs</param>
        /// <param name="position">Position of the fob</param>
        /// <param name="userId">User to assign the fob to</param>
        /// <returns></returns>
        Task AssignFobToUser(
            int systemId,
            int position,
            int userId);
    }
}
