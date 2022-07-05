using System.Collections.Generic;
using Traka.FobService.Model;

namespace Traka.FobService.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository()
            : base(new List<User>
            {
                new() {Id = 1, Username = "Sneezy"},
                new() {Id = 2, Username = "Sleepy"},
                new() {Id = 3, Username = "Grumpy"}
            })
        {
        }
    }
}
