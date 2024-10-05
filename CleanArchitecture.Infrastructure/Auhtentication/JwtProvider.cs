using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Domain.Entities;
using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Infrastructure.Auhtentication
{
    public sealed class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;
        public JwtProvider(IOptions<JwtOptions> jwtOptions)  // options pattern kullandıgımız için IOptions<> içerisine aldık
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string CreateToken(AppUser user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email),                
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),     // hepsi aynı şeye tekabül ediyor...
                new Claim("NameLastName", user.UserName)
            };

            JwtSecurityToken jwtSecurityToken = new(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256)
                );       

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }
    }
}

