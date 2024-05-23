using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private User user = new();
        private readonly UserDbService _databaseService = new();

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;

            if(_databaseService.InsertUser(user))
            {
                string token = CreateToken(user);
                return Ok(new { token }); ;
            }
            return BadRequest("user with this username already exitsts");
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request)
        {
            user = _databaseService.GetUser(request.Username);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    string token = CreateToken(user);
                    return Ok(new { token });
                }
            }
            return BadRequest("Username not found or password is wrong.");
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            //normally the key should be stored in some secure vault
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
