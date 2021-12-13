using System;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace NyitottKapukReg.Service.Auth
{
    public class JwtService
    {
        private readonly string _secret;
        private readonly string _expDate;

        public JwtService(IConfiguration config)
        {
            _secret = config.GetSection("JwtConfig").GetSection("secret").Value;
            _expDate = config.GetSection("JwtConfig").GetSection("expirationInMinutes").Value;
        }

        public string GenerateSecurityToken(string email, List<Claim> extraClaims = null)
        {
            var key = Encoding.ASCII.GetBytes(_secret);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email)
            };

            if (extraClaims != null && extraClaims.Count != 0)
                claims.AddRange(extraClaims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = new JwtSecurityToken(
                issuer: "Jedlik",
                audience: "NyitottKapuk",
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_expDate)),
                claims: claims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            return tokenHandler.WriteToken(jwt);
        }
    }
}