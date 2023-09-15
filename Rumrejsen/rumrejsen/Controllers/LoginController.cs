using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rumrejsen.Models;
using System.Data;
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
        public ActionResult<User> Register()
        {
            string password = "1234";
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);


            var Captain = new User
            {
                Username = "Captain",
                PasswordHash = passwordHash,
                Role = "Captain"
            };
            var Kadet = new User
            {
                Username = "Kadet",
                PasswordHash = passwordHash,
                Role = "Kadet"
            };



            // Retrieve or create the shopping cart in the session
            var Users = HttpContext.Session.GetObjectFromJson<List<User>>("Users") ?? new List<User>();

            Users.Add(Captain);
            Users.Add(Kadet);

            // Store the updated shopping cart in the session
            HttpContext.Session.SetObjectAsJson("Users", Users);

            return Ok(Users);
        }



        [HttpPost("login")]
        public ActionResult<User> Login([FromQuery] UserDTO request)
        {
            // Retrieve or create the shopping cart in the session
            var Users = HttpContext.Session.GetObjectFromJson<List<User>>("Users") ?? new List<User>();

            bool validUser = false;
            User loggedInUser = null; // Initialize a variable to store the logged-in user

            foreach (var user in Users)
            {
                if (user.Username == request.Username)
                {
                    validUser = true;
                    loggedInUser = user; // Store the logged-in user
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

            // Include the user's role in the JWT token
            string token = CreateToken(loggedInUser);

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> Claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role) // Add the user's role as a claim
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:SecretKey").Value!));

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
