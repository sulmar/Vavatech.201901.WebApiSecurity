using JWTAuthentication.IServices;
using JWTAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace JWTAuthentication.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("token")]
        public IHttpActionResult GenerateToken([FromBody] User userParam)
        {
            User user = usersService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
            {
                return BadRequest("Username or password is incorrect");
            }

            return Ok(user);
        }
    }
}