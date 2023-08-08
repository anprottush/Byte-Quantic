using CRUD_.Net_Core_Web_API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CRUD_.Net_Core_Web_API.Repositories
{
    public class TokenRepo
    {
        private readonly IConfiguration config;
        public TokenRepo(IConfiguration config)
        {
            this.config = config;
        }
        public string GenerateJwtToken()
        {
            var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]));
            var signin = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            var jwtsecuritytoken = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: null,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(config["Jwt:Expiration"])),
                signingCredentials: signin
                );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtsecuritytoken);
            return token;
        }
    }
}
