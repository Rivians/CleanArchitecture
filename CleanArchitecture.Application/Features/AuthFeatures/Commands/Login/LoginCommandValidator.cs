using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.AuthFeatures.Commands.Login
{
    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(p => p.UserNameOrEmail).NotEmpty().WithMessage("Kullanıcı adı yada email boş olamaz.");
            RuleFor(p => p.UserNameOrEmail).NotNull().WithMessage("Kullanıcı adı yada email boş olamaz.");
            RuleFor(p => p.UserNameOrEmail).MinimumLength(3).WithMessage("Kullanıcı adı yada email en az 3 karakter olmalıdır.");

            RuleFor(p => p.Password).NotEmpty().WithMessage("Şifre boş olamaz.");
            RuleFor(p => p.Password).NotNull().WithMessage("Şifre bilgisi boş olamaz.");
        }
    }
}
