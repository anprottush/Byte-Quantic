using CRUD_.Net_Core_Web_API.Models;
using CRUD_.Net_Core_Web_API.Models.DB;
using CRUD_.Net_Core_Web_API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CRUD_.Net_Core_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly ProjectDbContext db;
        public UserController(IConfiguration config, ProjectDbContext db)
        {
            this.config = config;
            this.db = db;
        }

        [HttpPost("registration")]
        public ActionResult Registration([FromBody] User user)
        {
            try
            {
                var extname = db.Users.Any(d => d.Username == user.Username);
                if (extname)
                {
                    return BadRequest("User Name is exists please try to another user name");
                }
                else
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return Ok("User register successful");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login(User user)
        {
            var userlogin = new UserRepo(db).Authenticate(user.Username,user.Password);
            if (userlogin != null)
            {
                var token = new TokenRepo(config).GenerateJwtToken();
                if (token != null)
                {
                    return Ok(new { Token = token, Time = DateTime.Now.ToString("F") });
                }
                else
                {
                    return Unauthorized(new { Message = "Invalid Token" });
                }
            }
            else
            {
                return BadRequest("Invalid User Please Register First");
            }

        }
    }
}
