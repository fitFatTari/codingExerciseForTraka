using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Traka.FobService.Model;
using Traka.FobService.Repositories;

namespace Traka.FobService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ISystemRepository _systemRepository;

        public UserController(IUserRepository userRepository,
            ISystemRepository systemRepository)
        {
            _userRepository = userRepository;
            _systemRepository = systemRepository;
        }

        // GET: /user
        [HttpGet]
        public Task<IEnumerable<User>> Get()
        {
            return _userRepository.GetAll();
        }

        // GET /user/5
        /// <summary>
        /// Retrieves a user record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A User</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
            {
                ModelState.AddModelError("id", "No user found with the given id");
                return NotFound(ModelState);
            }
            else
            {
                return Ok(user);
            }
        }

        // POST /user
        /// <summary>
        /// Adds the user to the database
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody] User value)
        {
            // validation
            // if validation fails return bad request

            _userRepository.Add(value);
        }

        // PUT /user/5
        /// <summary>
        /// Updates the user with the given ID with the name from the supplied user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns>Ok or Not Found http status</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] User value)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser is null) //this should be done by the service layer (using the OperationResult patter)
            {
                ModelState.AddModelError("id", "No user found with the given id");
                return NotFound(ModelState);
            }
            else
            {
                await _userRepository.Update(id, value);
                return Ok();
            }
        }

        // DELETE /user/5
        /// <summary>
        /// Removes the given user from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser is null) //this should be done by the service layer (using the OperationResult patter)
            {
                ModelState.AddModelError("id", "No user found with the given id");
                return NotFound(ModelState);
            }
            else
            {
                await _userRepository.Delete(id);
                return Ok();
            }
        }

        // GET /user/5/items
        /// <summary>
        /// Returns the fobs assigned to the given user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/fobs")]
        public async Task<IActionResult> Fobs(int id)
        {
            var systems = await _systemRepository.GetAll();
            var fobs = systems.SelectMany(s => s.Fobs.Where(f => f.CurrentUser.HasValue && f.CurrentUser.Value == id)).ToList();
            if (fobs.Count is 0)
            {
                ModelState.AddModelError("id", "No fobs found with for the given user id");
                return NotFound(ModelState);
            }
            else
            {
                return Ok(fobs);
            }
        }
    }
}
