using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
namespace Sqlsugar.Infrastructure.Helpers
{
    public interface IJWTService
    {
        string GetToken(string UserName);
    }
    public class JWTService : IJWTService
    {      
        public string GetToken(string UserName)
        {
            var Secret = Appsettings.app(new string[] { "JWT", "Secret" });
            var Audience = Appsettings.app(new string[] { "JWT", "Audience" });
            var SecurityKey = Appsettings.app(new string[] { "JWT", "SecurityKey" });
            var Issuer = Appsettings.app(new string[] { "JWT", "Issuer" });
            Claim[] claims = new[]
             {
                new Claim(ClaimTypes.Name, UserName),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
                new Claim("other","其他信息")
             };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),        //30分钟有效期
                signingCredentials: creds);
            string returnToken = new JwtSecurityTokenHandler().WriteToken(token);
            return returnToken;
        }
    }
}
