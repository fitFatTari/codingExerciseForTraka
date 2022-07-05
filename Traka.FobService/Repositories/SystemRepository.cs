using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Traka.FobService.Model;

namespace Traka.FobService.Repositories
{
    public class SystemRepository : BaseRepository<TrakaSystem>, ISystemRepository
    {


        public SystemRepository()
            : base(
                   new List<TrakaSystem>
                   {
                       new()
                       {
                           Id = 1,
                           Name = "System 1",
                           Fobs = new List<Fob>
                           {
                               new() {Position = 1, SerialNumber = "5D7C7C080000", CurrentStatus = FobStatus.InSystem},
                               new() {Position = 2, SerialNumber = "A6E77B080000", CurrentStatus = FobStatus.OutOfSystem, CurrentUser = 1}, 
                               new() {Position = 3, SerialNumber = "167E7C080000", CurrentStatus = FobStatus.OutOfSystem, CurrentUser = 2},
                               new() {Position = 4, SerialNumber = "F4422FFB4448", CurrentStatus = FobStatus.InSystem},
                               new() {Position = 5, SerialNumber = "243D7C080000", CurrentStatus = FobStatus.InSystem}
                           }
                       },
                       new()
                       {
                           Id = 2,
                           Name = "System 2",
                           Fobs = new List<Fob>
                           {
                               new() {Position = 1, SerialNumber = "A5CE14F9FDD4", CurrentStatus = FobStatus.InSystem},
                               new() {Position = 2, SerialNumber = "F9537C080000", CurrentStatus = FobStatus.InSystem},
                               new() {Position = 3, SerialNumber = "7AAFF3EF301F", CurrentStatus = FobStatus.InWrongPosition},
                               new() {Position = 4, SerialNumber = "1A4E265F130A", CurrentStatus = FobStatus.InSystem},
                               new() {Position = 5, SerialNumber = "BC2433AF8299", CurrentStatus = FobStatus.OutOfSystem, CurrentUser = 1}
                           }
                       }
                   })
        {
        }

        /// <inheritdoc cref="ISystemRepository.AssignFobToUser(int, int, int)"/>
        public async Task AssignFobToUser(
            int systemId,
            int position,
            int userId)
        {
            var system = (await GetAll()).SingleOrDefault(s => s.Id == systemId);

            if (system is null)
                throw new KeyNotFoundException("No system found with the given id");

            var fob = system.Fobs.SingleOrDefault(f => f.Position == position);

            if (fob is null)
                throw new KeyNotFoundException("No fob found with the given position");

            if (fob.CurrentStatus is not FobStatus.InSystem)
                throw new Exception("Fob status must be 'InSystem' in order to be assignable!");

            system.Fobs.Where(f => f.Position == position).ToList().ForEach(f => f.CurrentUser = userId);

            await Update(systemId, system);
        }
    }
}
