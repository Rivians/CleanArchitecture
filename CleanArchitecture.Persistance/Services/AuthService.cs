using AutoMapper;
using CleanArchitecture.Application.Features.AuthFeatures.Commands.Register;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity;
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
        public AuthService(UserManager<AppUser> userManager, IMapper mapper, IEmailService emailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
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
