
using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Infrastructure.Auhtentication;
using CleanArchitecture.WebApi.OptionsSetup;

namespace CleanArchitecture.WebApi.Configurations
{
    public sealed class InfrastructreServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJwtProvider, JwtProvider>();

            services.ConfigureOptions<JwtOptionsSetup>();  // ConfigureOptions<>, belirli ayarların (konfigürasyonların) merkezi bir noktadan uygulanmasını sağlar.
            services.ConfigureOptions<JwtBearerOptionsSetup>();
        }
    }
}
