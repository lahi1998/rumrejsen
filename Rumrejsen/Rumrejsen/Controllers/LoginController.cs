using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rumrejsen.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Rumrejsen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly List<User> Users = new List<User>();

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public ActionResult<User> Register([FromQuery] UserDTO request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);


            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash
            };

            // Retrieve or create the shopping cart in the session
            var Users = HttpContext.Session.GetObjectFromJson<List<User>>("Users") ?? new List<User>();

            Users.Add(newUser);

            // Store the updated shopping cart in the session
            HttpContext.Session.SetObjectAsJson("Users", Users);

            return Ok(Users);
        }

        [HttpGet("users")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            // Retrieve or create the shopping cart in the session
            var Users = HttpContext.Session.GetObjectFromJson<List<User>>("Users") ?? new List<User>();

            return Ok(Users);
        }

        [HttpPost("login")]
        public ActionResult<User> Login([FromQuery] UserDTO request)
        {
            // Retrieve or create the shopping cart in the session
            var Users = HttpContext.Session.GetObjectFromJson<List<User>>("Users") ?? new List<User>();

            bool validUser = false;

            foreach (var user in Users)
            {
                if (user.Username == request.Username)
                {
                    validUser = true;

                    if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                    {
                        return BadRequest("Password or username is wrong.");
                    }
                }
            }

            if (validUser == false)
            {
                return BadRequest("Password or username is wrong.");
            }


            string token = CreateToken(user);

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> Claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                    claims: Claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
