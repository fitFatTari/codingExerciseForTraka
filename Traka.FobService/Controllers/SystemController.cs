using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Traka.FobService.Model;
using Traka.FobService.Repositories;

namespace Traka.FobService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly ISystemRepository _systemRepository;

        public SystemController(ISystemRepository systemRepository)
        {
            _systemRepository = systemRepository;
        }

        // GET: /system
        [HttpGet]
        public Task<IEnumerable<TrakaSystem>> Get()
        {
            return _systemRepository.GetAll();
        }

        // PUT /system/{systemId}/assignfob/{position}/{userId}
        [HttpPut("{systemId}/assignfob/{position}/{userId}")]
        public async Task Put(int systemId, int position, int userId)
        {
            await _systemRepository.AssignFobToUser(systemId, position, userId);
        }
    }
}
