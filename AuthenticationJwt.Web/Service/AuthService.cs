using AuthenticationJwt.Web.Data;
using AuthenticationJwt.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationJwt.Web.Service
{
    public interface IAuthService
    {
        Task<ResponseModel> LoginAsync(LoginModel model);
        Task<ResponseModel> RegisterUser(UserModel model);
    }

    public class AuthService : IAuthService
    {
        private readonly MyAppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(MyAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ResponseModel> LoginAsync(LoginModel model)
        {
            try
            {
                var user = await _context.Users.Where(x => x.EmailAddress == model.Email).FirstOrDefaultAsync();
                if(user == null)
                {
                    return new()
                    {
                        IsSuccess = false,
                        Message = "User not found!"
                    };
                }

                if(!string.Equals(user.Password, model.Password))
                {
                    return new()
                    {
                        IsSuccess = false,
                        Message = "Password doesn't match!"
                    };
                }

                return new()
                {
                    IsSuccess = true,
                    Message = GenerateJsonWebToken(user)
                };

            }
            catch (Exception ex)
            {
                return new()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel> RegisterUser(UserModel model)
        {
            try
            {
                await _context.Users.AddAsync(model);   
                await _context.SaveChangesAsync();
                return new()
                {
                    IsSuccess = true,
                    Message = "Successfully Register!"
                };
            }
            catch(Exception  ex)
            {
                return new()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
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
