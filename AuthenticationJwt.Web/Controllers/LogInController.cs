using AuthenticationJwt.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationJwt.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LogInController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult LogIn([FromBody] UserModel user)
        {
            IActionResult response = Unauthorized();
            var authenticatedUser = AuthenticateUser(user);
            if (authenticatedUser != null)
            {
                var tokenString = GenerateJsonWebToken(authenticatedUser);
                response = Ok(new { token = tokenString });
            }
            return response;

        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;
            if (login.UserName.ToLower() == "maindeep")
            {
                user = new UserModel
                {
                    UserName = login.UserName,
                    EmailAddress = login.EmailAddress,
                    DateofJoining = login.DateofJoining


                };
                return user;
            }
        }


        private string GenerateJsonWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("DateofJoining", userInfo.DateofJoining.ToString()),
                new Claim("EmailAddress", userInfo.EmailAddress)
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
