﻿using CleanArchitecture.Application.Abstractions;
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
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using CleanArchitecture.Application.Features.AuthFeatures.Commands.Login;

namespace CleanArchitecture.Infrastructure.Auhtentication
{
    public sealed class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<AppUser> _userManager;
        public JwtProvider(IOptions<JwtOptions> jwtOptions, UserManager<AppUser> userManager)  // options pattern kullandıgımız için IOptions<> içerisine aldık
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
        }

        public async Task<LoginCommandResponse> CreateTokenAsync(AppUser user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email),                
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),     // hepsi aynı şeye tekabül ediyor...
                new Claim("NameLastName", user.UserName)
            };

            DateTime expires = DateTime.Now.AddHours(1); 

            JwtSecurityToken jwtSecurityToken = new(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256)
                );       

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpires = expires.AddMinutes(15);
            await _userManager.UpdateAsync(user);

            LoginCommandResponse response = new(
                token,
                refreshToken,
                user.RefreshTokenExpires,
                user.Id);

            return response;
        }
    }
}

