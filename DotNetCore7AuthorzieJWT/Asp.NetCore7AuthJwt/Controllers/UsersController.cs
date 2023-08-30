using Asp.NetCore7AuthJwt.Models;
using Asp.NetCore7AuthJwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asp.NetCore7AuthJwt.Controllers
{
    [Route("api/users")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [Produces("application/json")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get() => Ok(new { data = new List<string> { "value1", "value2" }});


        [HttpGet("list2")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<string>) )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        //Error in IActionResult =Task<IActionResult<IEnumerable<string>>>
        public async Task<ActionResult<IEnumerable<string>>> GetList2() => Ok(await Task.FromResult( new List<string> { "user1", "user2" }));


        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("list3")]
        [Authorize]
        public async Task<IEnumerable<string>> GetList3() => await Task.FromResult(new List<string> { "user1", "user2" });

        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("list4")]
        public async Task<IEnumerable<string>> GetList4() => await Task.FromResult(new List<string> { "user1", "user2" });

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public string Get(int id)
        {
            
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody]UserLoginDto userLogin,IOptions<JwtKeyOptions> jwtKeyOptionsOption)
        {
            if(userLogin == null) {  throw new ArgumentNullException(nameof(userLogin));}
            if (string.IsNullOrEmpty(userLogin.UserName) || string.IsNullOrEmpty(userLogin.Paswword))
                throw new ArgumentException("username and password is required");
            if(userLogin.UserName=="admin" && userLogin.Paswword=="admin")
            {
                var userInfo = new UserInfo()
                {
                    UserName = userLogin.UserName,
                    Email = "mojtaba.shagi@gmail.com",
                    FirstName = "mojtaba",
                    LastName = "shaghi",
                    UserId = 1
                };
                var userComplex = new UserComplexData
                {
                    ClientId = Guid.NewGuid().ToString(),
                    Identifier = Guid.NewGuid().ToString(),
                    RequestedOn = DateTime.UtcNow,
                    UserInfo = userInfo,
                };
               var token= JwtTokenGenerator.GenerateToken(userComplex, jwtKeyOptionsOption.Value);
                return Ok(await Task.FromResult( new { Token=token}));
            }
            return BadRequest("username or password is wrong");
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public void Delete(int id)
        {
        }
    }
}
