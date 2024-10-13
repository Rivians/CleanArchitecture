using AutoMapper;
using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Application.Features.AuthFeatures.Commands.CreateNewTokenByRefreshToken;
using CleanArchitecture.Application.Features.AuthFeatures.Commands.Login;
using CleanArchitecture.Application.Features.AuthFeatures.Commands.Register;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Persistance.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;  // identity kütüphanesinde manager islerini yapan class.
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IJwtProvider _jwtProvider;
        public AuthService(UserManager<AppUser> userManager, IMapper mapper, IEmailService emailService, IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
            _jwtProvider = jwtProvider;
        }

        public async Task<LoginCommandResponse> CreateTokenByRefreshTokenAsync(CreateNewTokenByRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("Kulanıcı bulunamadı!");

            if (user.RefreshToken != request.RefreshToken)
                throw new Exception("Refresh token geçerli değil!");

            if (user.RefreshTokenExpires < DateTime.Now)
                throw new Exception("Refresh tokenin süresi dolmuş");

            LoginCommandResponse response = await _jwtProvider.CreateTokenAsync(user);
            return response;
        }

        public async Task<LoginCommandResponse> LoginAsync(LoginCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.Users.Where(x => x.UserName == request.UserNameOrEmail || x.Email == request.UserNameOrEmail).FirstOrDefaultAsync(cancellationToken);

            if (user == null) throw new Exception("Kullanıcı bulunamadı!");

            var result = await _userManager.CheckPasswordAsync(user,request.Password);
            if (result)
            {
                LoginCommandResponse response = await _jwtProvider.CreateTokenAsync(user);
                return response;
            }

            throw new Exception("Şifreyi yanlış girdiniz!");
        }

        public async Task RegisterAsync(RegisterCommand request)
        {
            AppUser appUser = _mapper.Map<AppUser>(request);
            IdentityResult result = await _userManager.CreateAsync(appUser,request.Password);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            string email = request.Email;
            string body = "Kayıt işleminiz başarılıyla gerçekleşmiştir.";
            string subject = "Kayıt İşlemi";


            await _emailService.SendEmailAsync(email, subject, body);
        }
    }
}
